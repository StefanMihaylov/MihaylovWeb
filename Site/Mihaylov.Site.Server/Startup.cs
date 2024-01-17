using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mihaylov.Common.Host.Configurations;
using Mihaylov.Common.Abstract;
using Mihaylov.Site.Core;
using Mihaylov.Site.Database;
using Microsoft.EntityFrameworkCore;

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
            services.AddSwaggerCustom("v1", "v1", "Site API", null, true);

            services.AddHttpContextAccessor();
            services.AddLogging();

            services.AddControllers();

            services.AddMapping(Assembly.GetExecutingAssembly(), "Mihaylov.Site.Data.Contracts")
                    .AddCommon(ServiceLifetime.Singleton)
                    .AddSiteCore(opt =>
                    {
                        opt.ServerAddress = Environment.GetEnvironmentVariable("DB_Site_Address") ?? "192.168.1.100";
                        opt.DatabaseName = Environment.GetEnvironmentVariable("DB_Site_Name") ?? "Mihaylov_SiteDb";
                        opt.UserName = Environment.GetEnvironmentVariable("DB_Site_UserName");
                        opt.Password = Environment.GetEnvironmentVariable("DB_Site_Password");
                    },
                    siteOpt => siteOpt.SiteUrl = "http://"
                    );

            services.MigrateDatabase<SiteDbContext>(c => c.Migrate());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Site API V1");

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
