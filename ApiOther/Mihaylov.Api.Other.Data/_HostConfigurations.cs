using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Data.Cluster;
using Mihaylov.Api.Other.Data.Show;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services, 
            Action<NexusConfiguration> nexusConfig, Action<KubernetesSettings> kubeConfig,
            Action<VeleroSettings> veleroConfig)
        {
            services.AddScoped<IConcertService, ConcertService>();

            services.AddScoped<IClusterService, ClusterService>();
            services.AddScoped<IVersionService, VersionService>();

            services.Configure<KubernetesSettings>(kubeConfig);
            services.AddScoped<IKubernetesHelper, KubernetesHelper>();
            services.AddScoped<IVeleroService, VeleroService>();
            services.Configure<VeleroSettings>(veleroConfig);
            services.AddScoped<IVeleroClient, VeleroClient>();
            services.AddHttpClient(VeleroClient.VELERO_HTTP_CLIENT).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            services.AddHttpClient(NexusApiService.NEXUS_CLIENT_NAME).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });
            services.Configure<NexusConfiguration>(nexusConfig);
            services.AddScoped<INexusApiService, NexusApiService>();

            return services;
        }
    }
}
