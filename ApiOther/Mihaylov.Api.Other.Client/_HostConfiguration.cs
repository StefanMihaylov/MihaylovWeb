using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Api.Other.Client
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddOtherApiClient(this IServiceCollection services, string url)
        {
            services.AddHttpClient(OtherApiClient.OTHER_API_CLIENT_NAME, c =>
            {
                c.BaseAddress = new Uri(url);
                c.Timeout = TimeSpan.FromSeconds(60);
            });

            services.AddScoped<IOtherApiClient>(provider =>
            {
                var dependency = provider.GetRequiredService<IHttpClientFactory>();

                return new OtherApiClient(dependency);
            });

            return services;
        }
    }
}
