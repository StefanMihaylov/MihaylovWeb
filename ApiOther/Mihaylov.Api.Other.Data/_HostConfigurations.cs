using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Api.Other.Contracts.Gallery.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Data.Cluster;
using Mihaylov.Api.Other.Data.Gallery;
using Mihaylov.Api.Other.Data.Gallery.Models;
using Mihaylov.Api.Other.Data.Show;
using Mihaylov.Common.Generic.Extensions;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services, 
            Action<NexusConfiguration> nexusConfig, Action<KubernetesSettings> kubeConfig,
            Action<VeleroSettings> veleroConfig, Action<ImmichConfig> immichConfig)
        {
            services.AddScoped<IConcertService, ConcertService>();

            services.AddScoped<IClusterService, ClusterService>();
            services.AddScoped<IVersionService, VersionService>();

            services.Configure<KubernetesSettings>(kubeConfig);
            services.AddScoped<IKubernetesHelper, KubernetesHelper>();
            services.AddScoped<IVeleroService, VeleroService>();
            services.Configure<VeleroSettings>(veleroConfig);
            services.AddScoped<IVeleroClient, VeleroClient>();
            services.AddHttpClient(VeleroClient.VELERO_HTTP_CLIENT).IgnoreCertificate();

            services.AddHttpClient(NexusApiService.NEXUS_CLIENT_NAME).IgnoreCertificate();
            services.Configure<NexusConfiguration>(nexusConfig);
            services.AddScoped<INexusApiService, NexusApiService>();

            services.AddHttpClient(ImmichService.IMMICH_CLIENT_NAME).IgnoreCertificate();
            services.Configure<ImmichConfig>(immichConfig);
            services.AddScoped<IImmichService, ImmichService>();

            return services;
        }
    }
}
