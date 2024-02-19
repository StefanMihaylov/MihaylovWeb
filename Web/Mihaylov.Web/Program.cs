using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Api.Users.Client;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Common.Host;
using Mihaylov.Common.Host.Abstract.Authorization;
using Mihaylov.Common.Host.Abstract.Configurations;
using Mihaylov.Site.Media;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Areas.Identity.Pages.Account;
using Mihaylov.Web.Hubs;
using Mihaylov.Web.Models;
using Mihaylov.Web.Service;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            //  builder.Services.Configure<aaa>(configuration.GetSection("AppSettings"));

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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapHub<ScanProgressHub>("/ScanProgressHub");

            app.Run();
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .ConfigureApplicationPartManager(apm =>
                    {
                        var assembly = typeof(IdentityBuilderUIExtensions).Assembly;
                        var factory = new ConsolidatedAssemblyApplicationPartFactory();
                        var parts = factory.GetApplicationParts(assembly).ToList();

                        foreach (var part in parts)
                        {
                            apm.ApplicationParts.Add(part);
                        }

                        apm.FeatureProviders.Add(new ViewVersionFeatureProvider(assembly));
                    });

            services.AddRazorPages();
            services.AddSignalR();

            services.AddClientJwtAuthentication(LoginModel.COOKIE_NAME, ClaimType.Username, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER");
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE");
            });

            services.AddModuleInfo();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddUsersApiClient(Config.GetEnvironmentVariable("Users_Api_Client"));

            services.AddWeatherApiClient(Config.GetEnvironmentVariable("Weather_Api_Client"));
            services.AddScoped<IWeatherService, WeatherService>();
            services.Configure<WeatherConfig>(opt =>
            {
                var cities = Config.GetEnvironmentVariable("Weather_Api_Cities", "Sofia");

                opt.Cities = cities.Split(new char[] { ';', ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                opt.MetricUnits = Config.GetEnvironmentVariable<bool>("Weather_Api_MetricUnits", bool.TryParse, true);
                opt.Language = Config.GetEnvironmentVariable("Weather_Api_Language", "bg");
            });

            services.Configure<MediaConfig>(opt =>
            {
                opt.DuplicatePathLocked = Config.GetEnvironmentVariable("Media_Dub_Path_Lock", string.Empty);
                opt.DuplicatePathNotLocked = Config.GetEnvironmentVariable("Media_Dub_Path_NotLock", string.Empty);
            });

            services.AddMediaServices();

            services.AddScoped<IProgressReporterFactory, ProgressReporterFactory>();
        }
    }
}
