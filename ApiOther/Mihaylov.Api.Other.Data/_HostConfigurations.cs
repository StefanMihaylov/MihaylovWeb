using System;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Data.Cluster.Repositories;
using Mihaylov.Api.Other.Data.Cluster.Services;
using Mihaylov.Api.Other.Data.Show;
using Mihaylov.Api.Other.Data.Show.Repositories;
using Mihaylov.Api.Other.Data.Show.Services;
using Mihaylov.Api.Other.Database.Cluster;
using Mihaylov.Api.Other.Database.Shows;
using Mihaylov.Common.Abstract.Databases;

namespace Mihaylov.Api.Other.Data
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherDatabase(this IServiceCollection services, Action<ConnectionStringSettings> connectionString)
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            services.AddDbContext<MihaylovOtherShowDbContext>(options =>
            {
                options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
                {
                    opt.MigrationsHistoryTable("__MigrationsHistory", MihaylovOtherShowDbContext.SCHEMA_NAME);

                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });

            services.AddDbContext<MihaylovOtherClusterDbContext>(options =>
            {
                options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
                {
                    opt.MigrationsHistoryTable("__MigrationsHistory", MihaylovOtherClusterDbContext.SCHEMA_NAME);

                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });

            return services;
        }

        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddScoped<IConcertRepository, ConcertRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITicketProviderRepository, TicketProviderRepository>();
            services.AddScoped<IConcertService, ConcertService>();

            services.AddTransient<IMapper, Mapper>();
            services.RegisterDbMapping();

            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IPodRepository, PodRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IClusterService, ClusterService>();

            return services;
        }
    }
}
