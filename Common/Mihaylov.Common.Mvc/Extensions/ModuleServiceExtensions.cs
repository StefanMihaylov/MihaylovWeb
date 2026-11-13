using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
}
