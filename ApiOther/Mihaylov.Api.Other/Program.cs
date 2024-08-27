using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Data;
using Mihaylov.Api.Other.Database.Cluster;
using Mihaylov.Api.Other.Database.Shows;
using Mihaylov.Common.Abstract;
using Mihaylov.Common.Host;
using Mihaylov.Common.Host.Abstract.Configurations;

namespace Mihaylov.Api.Other
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
            services.AddSwaggerCustom("v1", "v1", "Other API", "Miscellaneous Endpoints API", false);

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
                    .AddOtherDatabase(opt =>
            {
                opt.ServerAddress = Config.GetEnvironmentVariable("DB_Other_Address", "192.168.1.100");
                opt.DatabaseName = Config.GetEnvironmentVariable("DB_Other_Name", "Mihaylov_OtherDb");
                opt.UserName = Config.GetEnvironmentVariable("DB_Other_UserName", "");
                opt.Password = Config.GetEnvironmentVariable("DB_Other_Password", "");
            });

            services.MigrateDatabase<MihaylovOtherShowDbContext>(c => c.Migrate());
            services.MigrateDatabase<MihaylovOtherClusterDbContext>(c => c.Migrate());

            services.AddOtherServices();

            services.AddClientJwtAuthentication(null, null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Other API V1");

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
