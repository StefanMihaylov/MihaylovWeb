using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
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
        /// <typeparam name="T">The concrete implementation of the API client.</typeparam>
        /// <typeparam name="TOptions">The options type containing API settings, implementing IBaseApiSettings.</typeparam>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="clientName">The logical name of the HTTP client.</param>
        /// <param name="moduleName">The module name, used as the last part of the section path.</param>
        /// <returns>The IHttpClientBuilder for further client configuration.</returns>
        public static IHttpClientBuilder AddApiClient<TClient, T, TOptions>(this IServiceCollection services, string clientName, string moduleName)
            where TClient : class where T : class, TClient where TOptions : class, IBaseApiSettings
        {
            services.TryAddTransient<BearerTokenHandler>();

            services.AddOptions<TOptions>().BindSettings(moduleName);

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
                return ActivatorUtilities.CreateInstance<T>(provider, provider.GetRequiredService<IHttpClientFactory>());
            });

            return builder;
        }

        /// <summary>
        /// Binds <typeparamref name="TOptions"/> to the <c>Modules:{moduleName}</c> section of the configuration,
        /// validates it with data annotations, and validates it when the host starts.
        /// </summary>
        /// <typeparam name="TOptions">The options type to bind.</typeparam>
        /// <param name="optionBuilder">The options builder to configure.</param>
        /// <param name="moduleName">The module name, used as the last part of the section path.</param>
        /// <remarks>
        /// Invalid or missing settings stop the application at startup with an <c>OptionsValidationException</c>
        /// instead of failing on the first request.
        /// </remarks>
        public static void BindSettings<TOptions>(this OptionsBuilder<TOptions> optionBuilder, string moduleName) where TOptions : class
        {
            optionBuilder.Configure<IConfiguration>((model, config) =>
            {
                config.GetSection($"Modules:{moduleName}").Bind(model);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();
        }
    }
}
