using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common.Generic.Extensions
{
    public static class HttpClientExtentions
    {
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
