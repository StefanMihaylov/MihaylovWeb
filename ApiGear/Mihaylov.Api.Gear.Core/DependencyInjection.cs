using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Gear.Core.Application;

namespace Mihaylov.Api.Gear.Core;

public class GearCoreConfig()
{
    public string MediatRLicenseKey { get; set; } = string.Empty;
};

public static class DependencyInjection
{
    public static IServiceCollection AddGearCqrs(this IServiceCollection services, Action<GearCoreConfig> configAction)
    {
        var config = new GearCoreConfig();
        configAction(config);

        services.AddMediatR(cfg => {
            cfg.LicenseKey = config.MediatRLicenseKey;
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.RegisterDbMapping();

        return services;
    }
}
