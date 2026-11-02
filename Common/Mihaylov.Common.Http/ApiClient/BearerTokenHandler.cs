using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Mihaylov.Common
{
    /// <summary>
    /// HTTP message handler that adds a Bearer token to outgoing requests using an access token provider.
    /// </summary>
    public class BearerTokenHandler : DelegatingHandler
    {
        // "Bearer" is the HTTP Authorization header scheme (RFC 6750). The API's JwtBearer
        // handler expects exactly this value in "Authorization: Bearer <token>".
        // It is a protocol constant, NOT the ASP.NET Core authentication scheme name
        // (UserConstants.AuthenticationScheme in Auth, which only happens to be spelled the same),
        // so it must not be linked to or changed together with that name.
        private const string AuthorizationScheme = "Bearer";

        private readonly IAccessTokenProvider _tokenProvider;

        /// <summary>
        /// Initializes a new instance of the BearerTokenHandler class using the specified access token provider.
        /// </summary>
        /// <param name="tokenProvider">The access token provider used to acquire bearer tokens.</param>
        public BearerTokenHandler(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Sends an HTTP request with a Bearer token added to the Authorization header if available.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var token = _tokenProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, token);
            }

            return base.SendAsync(request, ct);
        }
    }
}
