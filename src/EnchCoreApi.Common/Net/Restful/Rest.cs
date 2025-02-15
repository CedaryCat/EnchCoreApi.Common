﻿using EnchCoreApi.Common.Logger;
using HttpServer;
using HttpServer.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HttpListener = HttpServer.HttpListener;

namespace EnchCoreApi.Common.Net.Restful
{
    /// <summary>
    /// Rest command delegate
    /// </summary>
    /// <param name="args"><see cref="RestRequestArgs"/> object containing Verbs, Parameters, Request, and TokenData</param>
    /// <returns>Response object or null to not handle request</returns>
    public delegate object RestCommandD(RestRequestArgs args);

    /// <summary>
    /// A RESTful API service
    /// </summary>
    public class Rest : IDisposable
    {
        private readonly List<RestCommand> commands = [];
        /// <summary>
        /// Contains redirect URIs. The key is the base URI. The first item of the tuple is the redirect URI.
        /// The second item of the tuple is an optional "upgrade" URI which will be added to the REST response.
        /// </summary>
        private readonly Dictionary<string, Tuple<string, string?>> redirects = [];
        private HttpListener? listener;
        private readonly StringHeader serverHeader;
        private Timer tokenBucketTimer;
        private GenericLog Log;
        public Dictionary<string, TokenInfoBase> Tokens = [];
        /// <summary>
        /// Contains tokens used to manage REST authentication
        /// </summary>
        public Dictionary<string, int> tokenBucket = [];
        /// <summary>
        /// <see cref="IPAddress"/> the REST service is listening on
        /// </summary>
        public IPAddress Ip { get; set; }
        /// <summary>
        /// Port the REST service is listening on
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Rest"/> listening on the given IP and port
        /// </summary>
        /// <param name="ip"><see cref="IPAddress"/> to listen on</param>
        /// <param name="port">Port to listen on</param>
        public Rest(IPAddress ip, int port, GenericLog log) {
            Ip = ip;
            Port = port;
            AssemblyName assembly = this.GetType().Assembly.GetName();
            serverHeader = new StringHeader("Server", String.Format("{0}/{1}", assembly.Name, assembly.Version));
            Log = log;
        }

        /// <summary>
        /// Starts the RESTful API service
        /// </summary>
        public virtual bool Start(int requestIntervalMinutes = 10) {
            try {
                listener = HttpListener.Create(Ip, Port);
                listener.RequestReceived += OnRequest;
                listener.Start(int.MaxValue);
                tokenBucketTimer = new Timer((e) => {
                    DegradeBucket();
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(Math.Max(requestIntervalMinutes, 1)));
                return true;
            }
            catch (Exception ex) {
                Log.ConsoleErrorLog("Fatal Rest Startup Exception");
                Log.ConsoleErrorLog(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Starts the RESTful API service using the given <see cref="IPAddress"/> and port
        /// </summary>
        /// <param name="ip"><see cref="IPAddress"/> to listen on</param>
        /// <param name="port">Port to listen on</param>
        public bool Start(IPAddress ip, int port, GenericLog log) {
            Ip = ip;
            Port = port;
            Log = log;
            return Start();
        }

        /// <summary>
        /// Stops the RESTful API service
        /// </summary>
        public virtual void Stop() {
            listener?.Stop();
        }

        /// <summary>
        /// Registers a command using the given route
        /// </summary>
        /// <param name="path">URL route</param>
        /// <param name="callback">Command callback</param>
        public void Register(string path, RestCommandD callback) {
            AddCommand(new RestCommand(path, callback));
        }

        /// <summary>
        /// Registers a <see cref="RestCommand"/>
        /// </summary>
        /// <param name="com"><see cref="RestCommand"/> to register</param>
        public void Register(RestCommand com) {
            AddCommand(com);
        }

        /// <summary>
        /// Registers a redirection from a given REST route to a target REST route, with an optional upgrade URI
        /// </summary>
        /// <param name="baseRoute">The base URI that will be requested</param>
        /// <param name="targetRoute">The target URI to redirect to from the base URI</param>
        /// <param name="upgradeRoute">The upgrade route that will be added as an object to the <see cref="RestObject"/> response of the target route</param>
        /// <param name="parameterized">Whether the route uses parameterized querying or not.</param>
        public void RegisterRedirect(string baseRoute, string targetRoute, string? upgradeRoute = null, bool parameterized = true) {
            if (redirects.ContainsKey(baseRoute)) {
                redirects.Add(baseRoute, Tuple.Create(targetRoute, upgradeRoute));
            }
            else {
                redirects[baseRoute] = Tuple.Create(targetRoute, upgradeRoute);
            }
        }

        /// <summary>
        /// Adds a <see cref="RestCommand"/> to the service's command list
        /// </summary>
        /// <param name="com"><see cref="RestCommand"/> to add</param>
        protected void AddCommand(RestCommand com) {
            commands.Add(com);
        }

        private void DegradeBucket() {
            var _bucket = new List<string>(tokenBucket.Keys); // Duplicate the keys so we can modify tokenBucket whilst iterating
            foreach (string key in _bucket) {
                int tokens = tokenBucket[key];
                if (tokens > 0) {
                    tokenBucket[key] -= 1;
                }
                if (tokens <= 0) {
                    tokenBucket.Remove(key);
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="HttpListener"/> receives a request
        /// </summary>
        /// <param name="sender">Sender of the request</param>
        /// <param name="e">RequestEventArgs received</param>
        protected virtual void OnRequest(object? sender, RequestEventArgs e) {
            var obj = ProcessRequest(sender, e);
            if (obj == null)
                throw new NullReferenceException("obj");

            var str = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var jsonp = e.Request.Parameters["jsonp"];
            if (!string.IsNullOrWhiteSpace(jsonp)) {
                str = string.Format("{0}({1});", jsonp, str);
            }
            e.Response.Connection.Type = ConnectionType.Close;
            e.Response.ContentType = new ContentTypeHeader("application/json; charset=utf-8");
            e.Response.Add(serverHeader);
            var bytes = Encoding.UTF8.GetBytes(str);
            e.Response.Body.Write(bytes, 0, bytes.Length);
            e.Response.Status = HttpStatusCode.OK;
        }

        /// <summary>
        /// Attempts to process a request received by the <see cref="HttpListener"/>
        /// </summary>
        /// <param name="sender">Sender of the request</param>
        /// <param name="e">RequestEventArgs received</param>
        /// <returns>A <see cref="RestObject"/> describing the state of the request</returns>
        protected virtual object ProcessRequest(object? sender, RequestEventArgs e) {
            try {
                var uri = e.Request.Uri.AbsolutePath;
                uri = uri.TrimEnd('/');
                string? upgrade = null;

                if (redirects.ContainsKey(uri)) {
                    upgrade = redirects[uri].Item2;
                    uri = redirects[uri].Item1;
                }

                foreach (var cmd in commands) {
                    var verbs = new RestVerbs();
                    if (cmd.HasVerbs) {
                        var match = Regex.Match(uri, cmd.UriVerbMatch);
                        if (!match.Success)
                            continue;
                        if ((match.Groups.Count - 1) != cmd.UriVerbs.Length)
                            continue;

                        for (int i = 0; i < cmd.UriVerbs.Length; i++)
                            verbs.Add(cmd.UriVerbs[i], match.Groups[i + 1].Value);
                    }
                    else if (cmd.UriTemplate.ToLower() != uri.ToLower()) {
                        continue;
                    }

                    var obj = ExecuteCommand(cmd, verbs, e.Request.Parameters, e.Request, e.Context);
                    if (obj != null) {
                        if (!string.IsNullOrWhiteSpace(upgrade) && obj is RestObject restObj) {
                            if (!restObj.ContainsKey("upgrade")) {
                                restObj.Add("upgrade", upgrade);
                            }
                        }

                        return obj;
                    }
                }
            }
            catch (Exception exception) {
                return new RestObject("500")
                {
                    {"error", "Internal server error."},
                    {"errormsg", exception.Message},
                    {"stacktrace", exception.StackTrace},
                };
            }
            return new RestObject("404")
            {
                {"error", "Specified API endpoint doesn't exist. Refer to the documentation for a list of valid endpoints."}
            };
        }

        /// <summary>
        /// Executes a <see cref="RestCommand"/> using the provided verbs, parameters, request, and context objects
        /// </summary>
        /// <param name="cmd">The REST command to execute</param>
        /// <param name="verbs">The REST verbs used in the command</param>
        /// <param name="parms">The REST parameters used in the command</param>
        /// <param name="request">The HTTP request object associated with the command</param>
        /// <param name="context">The HTTP context associated with the command</param>
        /// <returns></returns>
        protected virtual object ExecuteCommand(RestCommand cmd, RestVerbs verbs, IParameterCollection parms, IRequest request, IHttpContext context) {
            if (!cmd.RequiresToken)
                return cmd.Execute(verbs, TokenInfoBase.None, parms, request, context);

            var token = parms["token"];
            if (token == null)
                return new RestObject("401") { Error = "Not authorized. The specified API endpoint requires a token." };

            if (!Tokens.TryGetValue(token, out var tokenData))
                return new RestObject("403") { Error = "Not authorized. The specified API endpoint requires a token, but the provided token was not valid." };

            if (tokenData.HasPermission(cmd.Permission)) {
                return new RestObject("403") { Error = string.Format("Not authorized. User \"{0}\" has no access to use the specified API endpoint.", tokenData.UserName) };
            }

            object result = cmd.Execute(verbs, tokenData, parms, request, context);
            if (cmd.DoLog)
                Log.ConsoleSuccessLog(string.Format("\"{0}\" requested REST endpoint: {1}", tokenData.UserName, this.BuildRequestUri(cmd, verbs, parms, false)));

            return result;
        }

        /// <summary>
        /// Builds a request URI from the parameters, verbs, and URI template of a <see cref="RestCommand"/>
        /// </summary>
        /// <param name="cmd">The REST command to take the URI template from</param>
        /// <param name="verbs">Verbs used in building the URI string</param>
        /// <param name="parms">Parameters used in building the URI string</param>
        /// <param name="includeToken">Whether or not to include a token in the URI</param>
        /// <returns></returns>
        protected virtual string BuildRequestUri(
            RestCommand cmd, RestVerbs verbs, IParameterCollection parms, bool includeToken = true
        ) {
            StringBuilder requestBuilder = new StringBuilder(cmd.UriTemplate);
            char separator = '?';
            foreach (var param in parms.OfType<Parameter>()) {
                if (param == null || (!includeToken && param.Name.Equals("token", StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                requestBuilder.Append(separator);
                requestBuilder.Append(param.Name);
                requestBuilder.Append('=');
                requestBuilder.Append(param.Value);
                separator = '&';
            }

            return requestBuilder.ToString();
        }

        #region Dispose

        /// <summary>
        /// Disposes the RESTful API service
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the RESTful API service
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (listener != null) {
                    listener.Stop();
                    listener = null;
                }
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Rest() {
            Dispose(false);
        }

        #endregion
    }
}
