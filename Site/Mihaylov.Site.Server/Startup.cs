using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddControllers();

            services.AddDICommon()
                    .AddSiteCore(opt =>
                    {
                        opt.ServerAddress = Environment.GetEnvironmentVariable("DB_Site_Address") ?? "192.168.1.7";
                        opt.DatabaseName = Environment.GetEnvironmentVariable("DB_Site_Name") ?? "Mihaylov_SiteDb";
                        opt.UserName = Environment.GetEnvironmentVariable("DB_Site_UserName") ?? "SA";
                        opt.Password = Environment.GetEnvironmentVariable("DB_Site_Password") ?? "Anubis12_4";
                    });

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
