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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Users API", Version = "v1" });
                //options.AddSwaggerAuthentication(UserConstants.AuthenticationScheme);
            });

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
            string basePath = Config.GetEnvironmentVariable("APP_PathPrefix", "/");
            basePath = $"/{basePath.Trim('/')}";

            //  app.InitializeUsersDb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var scheme = Config.GetEnvironmentVariable("APP_Scheme", httpReq.Scheme);
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}{basePath}" }
                    };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath.TrimEnd('/')}/swagger/v1/swagger.json", "Users API V1");
                c.RoutePrefix = string.Empty;
            });

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
