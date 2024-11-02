using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Api.Weather.Contracts.Interfaces;
using Mihaylov.Api.Weather.Data;
using Mihaylov.Api.Weather.Data.Configuration;
using Mihaylov.Common;

namespace Mihaylov.Api.Weather
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerCustom("v1", "v1", "Weather API", "Weather API", false);

            services.AddControllers();
            services.AddModuleInfo();
            services.AddMemoryCache();

            services.AddScoped<IWeatherApiService, WeatherApiService>();
            services.AddScoped<IWeatherManager, WeatherManager>();
            services.AddHttpClient(WeatherApiService.WEATHER_CLIENT, config =>
            {
                config.BaseAddress = new Uri(Config.GetEnvironmentVariable("WeatherApi_Url"));
            });

            services.Configure<WeatherApiSettings>(opt =>
            {
                opt.AppId = Config.GetEnvironmentVariable("WeatherApi_AppId");
            });

            services.Configure<WeatherConfig>(opt =>
            {
                var cities = Config.GetEnvironmentVariable("Weather_Api_Cities", "Sofia");
                opt.Cities = cities.Split(new char[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                opt.MetricUnits = Config.GetEnvironmentVariable<bool>("Weather_Api_MetricUnits", bool.TryParse, true);
                opt.Language = Config.GetEnvironmentVariable("Weather_Api_Language", "bg");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Weather API V1");

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
