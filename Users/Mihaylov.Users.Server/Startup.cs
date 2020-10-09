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
                options.AddSwaggerAuthentication(UserConstants.AuthenticationScheme);
            });

            services.AddControllers();            

            services.AddCommon()
                    .AddUserDatabase(opt =>
                        {
                            opt.ServerAddress = Environment.GetEnvironmentVariable("DB_Users_Address") ?? "192.168.1.7";
                            opt.DatabaseName = Environment.GetEnvironmentVariable("DB_Users_Name") ?? "Mihaylov_UsersDb";
                            opt.UserName = Environment.GetEnvironmentVariable("DB_Users_UserName");
                            opt.Password = Environment.GetEnvironmentVariable("DB_Users_Password");
                        })
                    .AddJwtAuthentication(opt =>
                       {
                           opt.Secret = Environment.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");
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
                app.UseDatabaseErrorPage();
            }

            app.UseHttpsRedirection();
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
