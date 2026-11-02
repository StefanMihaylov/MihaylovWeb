using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods for registering API clients and related dependencies using dependency injection.
    /// </summary>
    public static class HttpClientServiceExtensions
    {
        /// <summary>
        /// Registers a named <see cref="HttpClient"/> and a typed API client. The base address and timeout are read
        /// from <typeparamref name="TOptions"/> (<see cref="IBaseApiSettings"/>), and the client sends a bearer token
        /// through <see cref="BearerTokenHandler"/>.
        /// </summary>
        /// <typeparam name="TClient">The interface or base type of the API client.</typeparam>
        /// <typeparam name="TImpl">The concrete implementation of the API client.</typeparam>
        /// <typeparam name="TOptions">The options type containing API settings, implementing IBaseApiSettings.</typeparam>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="clientName">The logical name of the HTTP client.</param>
        /// <param name="bindSettings">Binds and validates <typeparamref name="TOptions"/>, 
        /// for example <c>builder =&gt; builder.BindSettings("Other")</c>. If it is <c>null</c>, the options are registered empty 
        /// and must be configured elsewhere.</param>
        /// <returns>The IHttpClientBuilder for further client configuration.</returns>
        public static IHttpClientBuilder AddApiClient<TClient, TImpl, TOptions>(this IServiceCollection services, string clientName,
            Action<OptionsBuilder<TOptions>> bindSettings)
            where TClient : class where TImpl : class, TClient where TOptions : class, IBaseApiSettings
        {
            services.TryAddTransient<BearerTokenHandler>();

            var optionsBuilder = services.AddOptions<TOptions>();
            if (bindSettings != null)
            {
                bindSettings(optionsBuilder);
            }

            var builder = services.AddHttpClient(clientName, (provider, http) =>
            {
                var settings = provider.GetRequiredService<IOptions<TOptions>>();
                var config = settings.Value;

                http.BaseAddress = new Uri(config.ApiUrl);
                http.Timeout = TimeSpan.FromSeconds(config.Timeout ?? 60);
            })
            .AddHttpMessageHandler<BearerTokenHandler>();

            services.AddTransient<TClient>(provider =>
            {
                return ActivatorUtilities.CreateInstance<TImpl>(provider, provider.GetRequiredService<IHttpClientFactory>());
            });

            return builder;
        }
    }
}
