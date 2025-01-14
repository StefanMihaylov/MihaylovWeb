using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Database;
using Mihaylov.Api.Site.Hubs;
using Mihaylov.Common;

namespace Mihaylov.Api.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddServices(builder.Services);

            var app = builder.Build();

            Configure(app);

            app.Run();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSwaggerCustom("v1", "v1", "Site API", "Site Endpoints API", false);

            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddModuleInfo();
            services.AddEndpointsApiExplorer();
            services.AddMemoryCache();

            services.AddClientJwtAuthentication(null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });

            services.AddDatabase<SiteDbContext>(opt =>
                    {
                        opt.ServerAddress = Config.GetEnvironmentVariable("DB_Site_Address", "192.168.1.100");
                        opt.DatabaseName = Config.GetEnvironmentVariable("DB_Site_Name", "Mihaylov_SiteDb");
                        opt.UserName = Config.GetEnvironmentVariable("DB_Site_UserName", "");
                        opt.Password = Config.GetEnvironmentVariable("DB_Site_Password", "");
                    })
                    .MigrateDatabase<SiteDbContext>(c => c.Migrate(), true)
                    .AddSiteRepositories()
                    .AddSiteServices(opt =>
                    {
                        opt.SiteUrl = Config.GetEnvironmentVariable("Site_SourceUrl");
                    });

            services.AddSignalR();
            services.AddScoped<IProgressReporterFactory, ProgressReporterFactory>();
            services.AddCors(options =>
            {
                options.AddPolicy("_SignalR_AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5281", "https://mihaylov-s.eu")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .SetIsOriginAllowed((x) => true)
                               .AllowCredentials();
                    });
            });
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Site API V1");

            app.UseRouting();
            app.UseCors("_SignalR_AllowSpecificOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapHub<UpdateProgressHub>("/UpdateProgressHub");            
        }
    }
}
