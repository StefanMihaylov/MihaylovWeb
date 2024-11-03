using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Dictionary.Database;
using Mihaylov.Common;

namespace Mihaylov.Api.Dictionary
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
            services.AddSwaggerCustom("v1", "v1", "Dictionary API", "Dictionary Endpoints API", false);

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
            // services.AddMemoryCache();

            services.AddClientJwtAuthentication(null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });

            services.AddDatabase<DictionaryDbContext>(opt =>
                    {
                        opt.ServerAddress = Config.GetEnvironmentVariable("DB_Dictionary_Address", "192.168.1.100");
                        opt.DatabaseName = Config.GetEnvironmentVariable("DB_Dictionary_Name", "Mihaylov_DictionaryDb");
                        opt.UserName = Config.GetEnvironmentVariable("DB_Dictionary_UserName", "");
                        opt.Password = Config.GetEnvironmentVariable("DB_Dictionary_Password", "");
                    })
                    .MigrateDatabase<DictionaryDbContext>(c => c.Migrate(), true)
                    .AddDictionaryRepositories()
                    .AddDictionaryServices();
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Dictionary API V1");

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
