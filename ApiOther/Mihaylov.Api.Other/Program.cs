using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Database.Cluster;
using Mihaylov.Api.Other.Database.Shows;
using Mihaylov.Common;

namespace Mihaylov.Api.Other
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
            services.AddSwaggerCustom("v1", "v1", "Other API", "Miscellaneous Endpoints API", false);

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
            services.AddMemoryCache();

            services.AddOtherDatabase(opt =>
                {
                    opt.ServerAddress = Config.GetEnvironmentVariable("DB_Other_Address", "192.168.1.100");
                    opt.DatabaseName = Config.GetEnvironmentVariable("DB_Other_Name", "Mihaylov_OtherDb");
                    opt.UserName = Config.GetEnvironmentVariable("DB_Other_UserName", "");
                    opt.Password = Config.GetEnvironmentVariable("DB_Other_Password", "");
                })
                .MigrateDatabase<MihaylovOtherShowDbContext>(c => c.Migrate(), true)
                .MigrateDatabase<MihaylovOtherClusterDbContext>(c => c.Migrate(), true)
                .AddOtherRepositories()
                .AddOtherServices(
                nexus =>
                {
                    nexus.BaseUrl = Config.GetEnvironmentVariable("Nexus_Base_Url");
                    nexus.Username = Config.GetEnvironmentVariable("Nexus_Username");
                    nexus.Password = Config.GetEnvironmentVariable("Nexus_Password");
                    nexus.RepositoryName = Config.GetEnvironmentVariable("Nexus_Repository", "docker-hosted");
                    nexus.SkippedVersionCount = Config.GetEnvironmentVariable("Nexus_Skipped_Version_Count", int.TryParse, 3);
                    nexus.SkippedVersionMonthsAge = Config.GetEnvironmentVariable<int>("Nexus_Skipped_Version_Months_Age", int.TryParse, 6);
                },
                kube =>
                {
                    kube.ServiceHost = Config.GetEnvironmentVariable("Kubernetes_Service_Host", string.Empty);
                    kube.ServicePort = Config.GetEnvironmentVariable("Kubernetes_Service_Port", "6443");
                    kube.ConfigPath = Config.GetEnvironmentVariable("Kubernetes_Config_Path", string.Empty);
                },
                veleroConfig =>
                {
                    veleroConfig.DownloadBasePath = Config.GetEnvironmentVariable("Velero_Download_BasePath", "https://github.com/vmware-tanzu/velero/releases/download");
                    veleroConfig.DownloadVersion = Config.GetEnvironmentVariable("Velero_Download_Version");
                    veleroConfig.DownloadFileName = Config.GetEnvironmentVariable("Velero_Download_FileName"); 
                    veleroConfig.TempPath = Config.GetEnvironmentVariable("Velero_Temp_Path");
                    veleroConfig.VeleroPath = Config.GetEnvironmentVariable("Velero_Path");
                    veleroConfig.CmdPath = Config.GetEnvironmentVariable("Velero_Cmd_Path", "cmd");
                    veleroConfig.CmdArguments = Config.GetEnvironmentVariable("Velero_Cmd_Arguments", "/c");
                });

            // Add-Migration <name> -Context MihaylovOtherClusterDbContext

            services.AddClientJwtAuthentication(null, opt =>
            {
                opt.Secret = Config.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET", "abc");
                opt.Issuer = Config.GetEnvironmentVariable("JWT_ISSUER", string.Empty);
                opt.Audience = Config.GetEnvironmentVariable("JWT_AUDIENCE", string.Empty);
            });
        }

        private static void Configure(IApplicationBuilder app)
        {
            app.UseSwaggerCustom("APP_Scheme", "APP_PathPrefix", "v1", "Other API V1");

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
