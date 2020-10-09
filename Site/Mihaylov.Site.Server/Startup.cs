using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mihaylov.Common;
using Mihaylov.Site.Core;
using Mihaylov.Site.Database;

namespace Mihaylov.Site.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Site API", Version = "v1" });
                options.AddSwaggerAuthentication("Bearer");
            });

            services.AddHttpContextAccessor();
            services.AddLogging();

            services.AddControllers();

            services.AddMapping(Assembly.GetExecutingAssembly(), "Mihaylov.Site.Data.Contracts")
                    .AddCommon(ServiceLifetime.Singleton)
                    .AddSiteCore(opt =>
                    {
                        opt.ServerAddress = Environment.GetEnvironmentVariable("DB_Site_Address") ?? "192.168.1.7";
                        opt.DatabaseName = Environment.GetEnvironmentVariable("DB_Site_Name") ?? "Mihaylov_SiteDb";
                        opt.UserName = Environment.GetEnvironmentVariable("DB_Site_UserName");
                        opt.Password = Environment.GetEnvironmentVariable("DB_Site_Password");
                    },
                    siteOpt => siteOpt.SiteUrl = "http://"
                    );

            services.MigrateDatabase<SiteDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Site API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
