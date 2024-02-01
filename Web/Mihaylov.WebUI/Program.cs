using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Api.Users.Client;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Common.Host.AssemblyVersion;
using Mihaylov.Common.Host.Configurations;
using Mihaylov.Web.Service;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Web.Service.Models;
using Mihaylov.Site.Media;
using Mihaylov.WebUI.Hubs;

namespace Mihaylov.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            //  builder.Services.Configure<aaa>(configuration.GetSection("AppSettings")); 150x150

            AddDependencies(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapHub<ScanProgressHub>("/ScanProgressHub");

            app.Run();
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            services.AddModuleInfo();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddUsersApiClient(Config.GetEnvironmentVariable("Users_Api_Client"));
            services.AddWeatherApiClient(Config.GetEnvironmentVariable("Weather_Api_Client"));

            services.AddScoped<IWeatherService, WeatherService>();
            services.Configure<WeatherConfig>(opt =>
            {
                var cities = Config.GetEnvironmentVariable("Weather_Api_Cities", "Sofia");

                opt.Cities = cities.Split(new char[] { ';', ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                opt.MetricUnits = Config.GetEnvironmentVariable<bool>("Weather_Api_MetricUnits", bool.TryParse ,true);
                opt.Language = Config.GetEnvironmentVariable("Weather_Api_Language", "bg");
            });

            services.AddMediaServices();

            services.AddScoped<IProgressReporterFactory, ProgressReporterFactory>();
        }
    }
}
