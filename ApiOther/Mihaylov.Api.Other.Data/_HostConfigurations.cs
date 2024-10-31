using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Data.Cluster;
using Mihaylov.Api.Other.Data.Show;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddScoped<IConcertService, ConcertService>();

            services.AddScoped<IClusterService, ClusterService>();
            services.AddScoped<IVersionService, VersionService>();

            return services;
        }
    }
}
