using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods for IHttpClientBuilder to configure HttpClient instances, including an option to
    /// ignore server certificate validation.
    /// </summary>
    public static class HttpClientExtentions
    {
        /// <summary>
        /// Configures the HTTP client's primary handler to accept all server SSL/TLS certificates, bypassing
        /// certificate validation.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to configure.</param>
        /// <returns>The same IHttpClientBuilder instance.</returns>
        public static IHttpClientBuilder IgnoreCertificate(this IHttpClientBuilder builder)
        {
            builder.ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            return builder;
        }
    }
}
