using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mihaylov.Common.Abstract;
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
            builder.Services.AddSwaggerCustom("v1", "v1", "Weather API", "Seek-Ah Weather API", false);

            var app = builder.Build();

            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Weather API V1");

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}