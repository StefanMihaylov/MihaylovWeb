using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mihaylov.Common;

/// <summary>
/// Extension methods for registering UI modules and binding their configuration.
/// </summary>
public static class ModuleServiceExtensions
{
    /// <summary>
    /// Collects the UI modules, lets each one register its own services, and registers an
    /// <see cref="IModuleCatalog"/> singleton that exposes the home menu, the identity (login) module and the
    /// other modules.
    /// </summary>
    /// <param name="services">The service collection to add the catalog and the module services to.</param>
    /// <param name="modulesAction">Adds the modules to the list it receives.</param>
    /// <returns>The same <paramref name="services"/> instance, for chaining.</returns>
    public static IServiceCollection RegisterModules(this IServiceCollection services, Action<List<IUiModule>> modulesAction)
    {
        var modules = new List<IUiModule>();
        modulesAction(modules);

        foreach (var module in modules)
        {
            module.ConfigureServices(services);
        }

        services.AddSingleton<IModuleCatalog>(new ModuleCatalog(modules));

        return services;
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
