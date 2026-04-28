using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core;
using Mihaylov.Api.Gear.Infrastructure;
using Mihaylov.Api.Gear.Infrastructure.Persistence;
using Mihaylov.Common;

namespace Mihaylov.Api.Gear
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

        private static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSwaggerCustom("v1", "v1", "Gear API", "Gear API", false);

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

            services.AddGearDatabase(opt =>
            {
                opt.ServerAddress = Config.GetEnvironmentVariable("DB_Gear_Address", "192.168.1.100");
                opt.DatabaseName = Config.GetEnvironmentVariable("DB_Gear_Name", "Mihaylov_GearDb");
                opt.UserName = Config.GetEnvironmentVariable("DB_Gear_UserName", "sa");
                opt.Password = Config.GetEnvironmentVariable("DB_Gear_Password", "Anubis12_4");
            })
            .MigrateDatabase<MihaylovGearDbContext>(c => c.Migrate(), true)
            .AddGearCqrs(opt =>
            {
                opt.MediatRLicenseKey = Config.GetEnvironmentVariable("MediatR_LicenseKey", string.Empty);
            });

            services.AddClientJwtAuthentication(null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });

            return services;
        }

        private static void Configure(IApplicationBuilder app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Gear API V1");

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
