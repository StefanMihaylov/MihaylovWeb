using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Databases;

namespace Mihaylov.Site.Database
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddSiteDatabase(this IServiceCollection services, Action<ConnectionStringSettings> connectionString)
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            services.AddDbContext<SiteDbContext>(options =>
                        options.UseSqlServer(connectionStringSettings.GetConnectionString()));

            return services;
        }
    }
}
