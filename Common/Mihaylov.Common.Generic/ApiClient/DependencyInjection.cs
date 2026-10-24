using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods for registering API clients and related dependencies using dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Adds a typed HTTP API client with configuration bound from a module-specific section and registers a bearer
        /// token handler.
        /// </summary>
        /// <typeparam name="TClient">The interface or base type of the API client.</typeparam>
        /// <typeparam name="TImpl">The concrete implementation of the API client.</typeparam>
        /// <typeparam name="TOptions">The options type containing API settings, implementing IBaseApiSettings.</typeparam>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="clientName">The logical name of the HTTP client.</param>
        /// <param name="bindSettings"></param>
        /// <returns>The IHttpClientBuilder for further client configuration.</returns>
        // The only place that knows the section convention: Modules:<ModuleName>
        public static IHttpClientBuilder AddApiClient<TClient, TImpl, TOptions>(this IServiceCollection services, string clientName,
            Action<OptionsBuilder<TOptions>> bindSettings)
            where TClient : class where TImpl : class, TClient where TOptions : class, IBaseApiSettings
        {
            services.TryAddTransient<BearerTokenHandler>();

            var optionBbuilder = services.AddOptions<TOptions>();
            if (bindSettings != null)
            {
                bindSettings(optionBbuilder);
            }

            var builder = services.AddHttpClient<TClient, TImpl>(clientName, (sp, http) =>
            {
                var settings = sp.GetRequiredService<IOptions<TOptions>>();
                var config = settings.Value;

                http.BaseAddress = new Uri(config.ApiUrl);
                http.Timeout = TimeSpan.FromSeconds(config.Timeout ?? 60);
            })
            .AddHttpMessageHandler<BearerTokenHandler>();

            return builder;
        }
    }
}
