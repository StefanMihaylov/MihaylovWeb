using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Gear;
using Mihaylov.UI.Mvc.Home;
using Mihaylov.UI.Mvc.Home.Hubs;
using Mihaylov.UI.Mvc.Other;
using Mihaylov.UI.Mvc.Site;
using Mihaylov.UI.Mvc.Users;

namespace Mihaylov.Web
{
    public class Program
    {
        private const string LOGIN_URL = "/Identity/Account/Login";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddDependencies(builder.Services);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/robots.txt"))
                {
                    string output = "User-agent: * \nAllow: /$ \nDisallow: /";
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(output);
                }
                else await next();
            });

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication()
               .UseStatusCodePages(async context =>
               {
                   var response = context.HttpContext.Response;

                   if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                       response.StatusCode == (int)HttpStatusCode.Forbidden)
                   {
                       response.Redirect(LOGIN_URL);
                   }

                   await Task.CompletedTask;
               })
               .UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapHub<ScanProgressHub>(ScanProgressHub.HUB_PATH);

            app.Run();
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .ConfigureApplicationPartManager(apm =>
                    {
                        apm.AddUsersIdentityPages();
                    });

            services.AddRazorPages();
            services.AddMemoryCache();

            services.AddClientJwtAuthentication(h =>
            {
                h.CookieName = IAccessTokenProvider.COOKIE_NAME;
                h.LoginUrl = LOGIN_URL;
            },
            opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER");
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE");
                opt.NameClaimType = ClaimMap.GetClaim(Config.GetEnvironmentVariable("JWT_NameClaim", string.Empty));
            });

            services.AddModuleInfo();
            services.RegisterModules(c =>
            {
                c.Add(new HomeUiModule());
                c.Add(new UsersUiModule());
                c.Add(new OtherUiModule());
                c.Add(new SiteUiModule());
                c.Add(new GearUiModule());
            });
        }
    }
}
