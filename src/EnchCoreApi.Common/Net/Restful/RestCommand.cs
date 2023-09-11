using HttpServer;
using System.Text.RegularExpressions;

namespace EnchCoreApi.Common.Net.Restful {
    public class RestCommand {
        public string Name { get; protected set; }
        public string UriTemplate { get; protected set; }
        public string UriVerbMatch { get; protected set; }
        public string[] UriVerbs { get; protected set; }
        public bool RequiresToken { get; protected set; }
        public string Permission { get; protected set; }
        public bool DoLog { get; set; }

        private RestCommandD callback;

        /// <summary>
        /// Creates a new <see cref="RestCommand"/> used with the REST API
        /// </summary>
        /// <param name="name">Used for identification</param>
        /// <param name="uritemplate">Url template</param>
        /// <param name="callback">Rest Command callback</param>
        public RestCommand(string name, string uritemplate, RestCommandD callback) {
            Name = name;
            UriTemplate = uritemplate;
            UriVerbMatch = string.Format("^{0}$", string.Join("([^/]*)", Regex.Split(uritemplate, "\\{[^\\{\\}]*\\}")));
            var matches = Regex.Matches(uritemplate, "\\{([^\\{\\}]*)\\}");
            UriVerbs = (from Match match in matches select match.Groups[1].Value).ToArray();
            this.callback = callback;
            DoLog = true;
        }

        /// <summary>
        /// Creates a new <see cref="RestCommand"/> used with the REST API
        /// </summary>
        /// <param name="uritemplate">Url template</param>
        /// <param name="callback">Rest Command callback</param>
        public RestCommand(string uritemplate, RestCommandD callback)
            : this(string.Empty, uritemplate, callback) {
        }

        public bool HasVerbs {
            get { return UriVerbs.Length > 0; }
        }

        public object Execute(RestVerbs verbs, TokenInfoBase tokenData, IParameterCollection parameters, IRequest request, IHttpContext context) {
            if (tokenData.Equals(TokenInfoBase.None) && RequiresToken)
                return new RestObject("401") { Error = "Not authorized. The specified API endpoint requires a token." };
            return callback(new RestRequestArgs(verbs, parameters, request, tokenData, context));
        }
    }
}
