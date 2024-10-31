using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Users.Client;

namespace Mihaylov.Api
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddUsersApiClient(this IServiceCollection services, string url)
        {
            services.AddHttpClient(UsersApiClient.USERS_API_CLIENT_NAME, c =>
            {
                c.BaseAddress = new Uri(url);
            });

            services.AddScoped<IUsersApiClient>(provider =>
            {
                var dependency = provider.GetRequiredService<IHttpClientFactory>();

                return new UsersApiClient(dependency);
            });

            return services;
        }
    }
}
