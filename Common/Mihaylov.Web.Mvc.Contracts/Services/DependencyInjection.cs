using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mihaylov.Common;

public static partial class DependencyInjection
{
    public static IServiceCollection RegisterModules(this IServiceCollection services, MenuItem home, MenuItem login, Action<List<IUiModule>> modulesAction)
    {
        var modules = new List<IUiModule>();
        modulesAction(modules);

        services.AddSingleton<IModuleCatalog>(new ManualModuleCatalog(home, login, modules));

        foreach (var module in modules)
        {
            module.ConfigureServices(services);
        }

        return services;
    }

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
