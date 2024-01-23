using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public static class HostConfiguration
    {
        public static IServiceCollection AddModuleInfo(this IServiceCollection services, Assembly assembly = null)
        {
            services.Configure<AssemblyWrapper>(ar =>
            {
                ar.Assembly = assembly ?? Assembly.GetEntryAssembly();
            });

            services.AddScoped<IModuleAssemblyService, ModuleAssemblyService>();
            services.AddScoped<ISystemConfiguration, SystemConfiguration>();

            return services;
        }
    }
}
