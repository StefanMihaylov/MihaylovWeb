using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Api.Weather.Client
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddWeatherApiClient(this IServiceCollection services, string url)
        {
            services.AddHttpClient(WeatherApiClient.WEATHER_API_CLIENT_NAME, c =>
            {
                c.BaseAddress = new Uri(url);
            });

            services.AddScoped<IWeatherApiClient>(provider =>
            {
                var dependency = provider.GetRequiredService<IHttpClientFactory>();

                return new WeatherApiClient(dependency);
            });

            return services;
        }
    }
}
