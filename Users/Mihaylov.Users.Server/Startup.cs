using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mihaylov.Common.Abstract;
using Mihaylov.Users.Data;
using Mihaylov.Users.Data.Database;

namespace Mihaylov.Users.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerCustom("v1", "v1", "Users API", null, false);

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllers();

            services.AddCommon()
                    .AddUserDatabase(opt =>
                        {
                            opt.ServerAddress = Config.GetEnvironmentVariable("DB_Users_Address");
                            opt.DatabaseName = Config.GetEnvironmentVariable("DB_Users_Name");
                            opt.UserName = Config.GetEnvironmentVariable("DB_Users_UserName");
                            opt.Password = Config.GetEnvironmentVariable("DB_Users_Password");
                        },
                        password =>
                        {
                            password.RequireNonAlphanumeric = Config.GetEnvironmentVariable("Password_RequireNonAlphanumeric", bool.TryParse, false);
                            password.RequireUppercase = Config.GetEnvironmentVariable("Password_RequireUppercase", bool.TryParse, false);
                            password.RequireDigit = Config.GetEnvironmentVariable("Password_RequireDigit", bool.TryParse, false);
                            password.RequiredLength = Config.GetEnvironmentVariable("Password_RequiredLength", int.TryParse, 6);
                        })
                    .AddJwtAuthentication(opt =>
                       {
                           opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");
                           opt.ExpiresIn = Config.GetEnvironmentVariable("JWT_ExpiresIn", int.TryParse, 14);
                           opt.ClaimTypes = Config.GetEnvironmentVariable("JWT_Claims", int.TryParse, 0);
                       });

            services.MigrateDatabase<MihaylovUsersDbContext>();
            services.InitializeUsersDb();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //  app.InitializeUsersDb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Users API V1");

            //app.UseHttpsRedirection();
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
