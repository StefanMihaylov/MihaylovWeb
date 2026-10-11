using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Api.Other.Contracts.Gallery.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Weather.Interfaces;
using Mihaylov.Api.Other.Data.Cluster;
using Mihaylov.Api.Other.Data.Gallery;
using Mihaylov.Api.Other.Data.Gallery.Models;
using Mihaylov.Api.Other.Data.Show;
using Mihaylov.Api.Other.Data.Weather;
using Mihaylov.Api.Other.Data.Weather.Configuration;
using Mihaylov.Common;

namespace Mihaylov.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services,
            Action<NexusConfiguration> nexusConfig, Action<KubernetesSettings> kubeConfig,
            Action<ProcessSettings> processConfig, Action<VeleroSettings> veleroConfig, 
            Action<KopiaSettings> kopiaSettings, Action<ImmichConfig> immichConfig,
            Action<PlaywrightSettings> playwrightSettings, Action<WeatherConfig> weatherConfig)
        {
            services.AddScoped<IConcertService, ConcertService>();

            services.AddScoped<IClusterService, ClusterService>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddSingleton<ISemaphoreProvider, SemaphoreProvider>();
            services.Configure<PlaywrightSettings>(playwrightSettings);
            services.AddSingleton<IPlaywrightManager, PlaywrightManager>();

            services.Configure<ProcessSettings>(processConfig);
            services.AddScoped<IProcessHelper, ProcessHelper>();
            services.Configure<KubernetesSettings>(kubeConfig);
            services.AddScoped<IKubernetesHelper, KubernetesHelper>();
            services.AddScoped<IVeleroService, VeleroService>();
            services.Configure<VeleroSettings>(veleroConfig);
            services.AddScoped<IVeleroClient, VeleroClient>();
            services.Configure<KopiaSettings>(kopiaSettings);
            services.AddScoped<IKopiaClient, KopiaClient>();

            services.AddHttpClient(NexusApiService.NEXUS_CLIENT_NAME).IgnoreCertificate();
            services.Configure<NexusConfiguration>(nexusConfig);
            services.AddScoped<INexusApiService, NexusApiService>();

            services.AddHttpClient(ImmichService.IMMICH_CLIENT_NAME).IgnoreCertificate();
            services.Configure<ImmichConfig>(immichConfig);
            services.AddScoped<IImmichService, ImmichService>();

            services.AddScoped<IWeatherApiService, WeatherApiService>();
            services.AddScoped<IWeatherManager, WeatherManager>();

            var weatherSettings = new WeatherConfig();
            weatherConfig(weatherSettings);

            services.AddHttpClient(WeatherApiService.WEATHER_CLIENT, config =>
            {
                config.BaseAddress = new Uri(weatherSettings.BaseUrl);
            });

            services.Configure<WeatherApiSettings>(opt =>
            {
                opt.AppId = weatherSettings.AppId;
            });

            services.Configure<WeatherConfig>(conf =>
            {
                conf.BaseUrl = weatherSettings.BaseUrl;
                conf.AppId = weatherSettings.AppId;
                conf.Cities = weatherSettings.Cities;
                conf.MetricUnits = weatherSettings.MetricUnits;
                conf.Language = weatherSettings.Language;
            });

            return services;
        }

        //public static void InitializeOtherDependencies<T>(this IApplicationBuilder app)
        //{
        //    var exitCode = Microsoft.Playwright.Program.Main(["install", "--with-deps", "chromium"]);
        //    if (exitCode != 0)
        //    {
        //        throw new Exception("Playwright browser installation failed.");
        //    }

        //    var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
        //    var logger = factory.CreateLogger(typeof(T));

        //    logger.LogInformation("Playwright was configured.");
        //}
    }
}
