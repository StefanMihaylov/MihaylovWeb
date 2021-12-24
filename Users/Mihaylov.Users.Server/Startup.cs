using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mihaylov.Common;
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
                            opt.ServerAddress = Environment.GetEnvironmentVariable("DB_Users_Address") ?? throw new ArgumentException("DB_Users_Address missing");
                            opt.DatabaseName = Environment.GetEnvironmentVariable("DB_Users_Name") ?? throw new ArgumentException("DB_Users_Name missing");
                            opt.UserName = Environment.GetEnvironmentVariable("DB_Users_UserName") ?? throw new ArgumentException("DB_Users_UserName missing");
                            opt.Password = Environment.GetEnvironmentVariable("DB_Users_Password") ?? throw new ArgumentException("DB_Users_Password missing");
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
                           opt.Secret = Environment.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET") ?? throw new ArgumentException("JWT_AUTHENTICATION_SECRET missing");
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
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
                c.RoutePrefix = string.Empty;
            });

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
