using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Api
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddSiteApiClient(this IServiceCollection services, string url)
        {
            services.AddHttpClient(SiteApiClient.SITE_API_CLIENT_NAME, c =>
            {
                c.BaseAddress = new Uri(url);
                c.Timeout = TimeSpan.FromSeconds(60);
            });

            services.AddScoped<ISiteApiClient>(provider =>
            {
                var dependency = provider.GetRequiredService<IHttpClientFactory>();

                return new SiteApiClient(dependency);
            });

            return services;
        }
    }
}
