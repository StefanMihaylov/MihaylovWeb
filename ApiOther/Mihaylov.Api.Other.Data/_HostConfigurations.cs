using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Nexus.Interfaces;
using Mihaylov.Api.Other.Contracts.Nexus.Models;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Data.Cluster;
using Mihaylov.Api.Other.Data.Nexus;
using Mihaylov.Api.Other.Data.Show;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services, 
            Action<NexusConfiguration> nexusConfig)
        {
            services.AddScoped<IConcertService, ConcertService>();

            services.AddScoped<IClusterService, ClusterService>();
            services.AddScoped<IVersionService, VersionService>();

            services.AddHttpClient(NexusApiService.NEXUS_CLIENT_NAME);
            services.Configure<NexusConfiguration>(nexusConfig);
            services.AddScoped<INexusApiService, NexusApiService>();

            return services;
        }
    }
}
