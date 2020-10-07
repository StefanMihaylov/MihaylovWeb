using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Web.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using Mihaylov.Site.Core;
using Mihaylov.Dictionaries.Core;
using Mihaylov.Web.Common.Toastr;

namespace Mihaylov.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public IContainer ApplicationContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddControllersAsServices();

            string connectionString = Configuration.GetConnectionString("Local");
            string siteUrl = Configuration.GetValue<string>("SiteUrl");
            string loggerPath = Configuration.GetValue<string>("LoggerPath");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
           // services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentitySettings();            

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<ToastrHelper>().As<IToastrHelper>();
            //builder.RegisterModule(new LoggingModule());
            //builder.RegisterModule(new AutoFacModuleSiteCore(connectionString, siteUrl));
            builder.RegisterModule(new AutofacModuleDictionariesCore(connectionString));

            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
