using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WeatherApi.Interfaces;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Programs
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddScoped<IWeatherService, WeatherService>();

            builder.Services.AddHttpClient("Weather", config =>
            {
                config.BaseAddress = new Uri("http://api.weatherapi.com");
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Weather API",
                    Description = "Seek-Ah Weather API",
                });
            });

            var app = builder.Build();

            string basePath = Environment.GetEnvironmentVariable("APP_PathPrefix") ?? "/";
            basePath = $"/{basePath.Trim('/')}";

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var scheme = Environment.GetEnvironmentVariable("APP_Scheme") ?? httpReq.Scheme;
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}{basePath}" }
                    };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath.TrimEnd('/')}/swagger/v1/swagger.json", "Users API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}