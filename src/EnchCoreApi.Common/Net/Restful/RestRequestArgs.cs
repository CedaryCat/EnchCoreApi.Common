using HttpServer;

namespace EnchCoreApi.Common.Net.Restful
{

    /// <summary>
    /// Describes the data contained in a REST request
    /// </summary>
    public class RestRequestArgs
    {
        /// <summary>
        /// Verbs sent in the request
        /// </summary>
        public RestVerbs Verbs { get; private set; }
        /// <summary>
        /// Parameters sent in the request
        /// </summary>
        public IParameterCollection Parameters { get; private set; }
        /// <summary>
        /// The HTTP request
        /// </summary>
        public IRequest Request { get; private set; }
        /// <summary>
        /// Token data used by the request
        /// </summary>
        public TokenInfoBase TokenData { get; private set; }
        /// <summary>
        /// <see cref="IHttpContext"/> used by the request
        /// </summary>
        public IHttpContext Context { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="RestRequestArgs"/> with the given verbs, parameters, request, and context.
        /// No token data is used
        /// </summary>
        /// <param name="verbs">Verbs used in the request</param>
        /// <param name="param">Parameters used in the request</param>
        /// <param name="request">The HTTP request</param>
        /// <param name="context">The HTTP context</param>
        public RestRequestArgs(RestVerbs verbs, IParameterCollection param, IRequest request, IHttpContext context) {
            Verbs = verbs;
            Parameters = param;
            Request = request;
            TokenData = TokenInfoBase.None;
            Context = context;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RestRequestArgs"/> with the given verbs, parameters, request, token data, and context.
        /// </summary>
        /// <param name="verbs">Verbs used in the request</param>
        /// <param name="param">Parameters used in the request</param>
        /// <param name="request">The HTTP request</param>
        /// <param name="tokenData">Token data used in the request</param>
        /// <param name="context">The HTTP context</param>
        public RestRequestArgs(RestVerbs verbs, IParameterCollection param, IRequest request, TokenInfoBase tokenData, IHttpContext context) {
            Verbs = verbs;
            Parameters = param;
            Request = request;
            TokenData = tokenData;
            Context = context;
        }
    }
}
