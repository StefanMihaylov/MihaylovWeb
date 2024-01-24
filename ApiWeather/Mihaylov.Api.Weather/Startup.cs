using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Api.Weather.Contracts.Interfaces;
using Mihaylov.Api.Weather.Data;
using Mihaylov.Api.Weather.Data.Configuration;
using Mihaylov.Common.Host.AssemblyVersion;
using Mihaylov.Common.Host.Configurations;

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
            services.AddSwaggerCustom("v1", "v1", "Weather API", "Seek-Ah Weather API", false);

            services.AddControllers();
            services.AddModuleInfo();

            services.AddScoped<IWeatherApiService, WeatherApiService>();
            services.AddHttpClient(WeatherApiService.WEATHER_CLIENT, config =>
            {
                config.BaseAddress = new Uri(Config.GetEnvironmentVariable("WeatherApi_Url"));
            });

            services.Configure<WeatherApiSettings>(opt =>
            {
                opt.AppId = Config.GetEnvironmentVariable("WeatherApi_AppId");
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
