using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Data;
using Mihaylov.Common.Abstract;
using Mihaylov.Common.Host;
using Mihaylov.Common.Host.Abstract.Configurations;

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
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddCommon()
                    .AddSiteDatabase(opt =>
                    {
                        opt.ServerAddress = Config.GetEnvironmentVariable("DB_Site_Address");
                        opt.DatabaseName = Config.GetEnvironmentVariable("DB_Site_Name");
                        opt.UserName = Config.GetEnvironmentVariable("DB_Site_UserName");
                        opt.Password = Config.GetEnvironmentVariable("DB_Site_Password");
                    });

            // services.MigrateDatabase<MihaylovOtherShowDbContext>(c => c.Migrate());

            services.AddSiteServices(opt =>
            {
                opt.SiteUrl = "";
            });

            services.AddClientJwtAuthentication(null, null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });

            using var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<IMigrateService>();
            service.Run();
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Site API V1");

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
