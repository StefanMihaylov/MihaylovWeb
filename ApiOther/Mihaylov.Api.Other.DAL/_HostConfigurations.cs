using System;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.DAL;
using Mihaylov.Api.Other.DAL.Cluster;
using Mihaylov.Api.Other.DAL.Show;
using Mihaylov.Api.Other.Database.Cluster;
using Mihaylov.Api.Other.Database.Shows;
using Mihaylov.Common;
using Mihaylov.Common.Database.Models;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddOtherDatabase(this IServiceCollection services,
                Action<ConnectionStringSettings> connectionString)
        {
            services.AddDatabase<MihaylovOtherShowDbContext>(connectionString);
            services.AddDatabase<MihaylovOtherClusterDbContext>(connectionString);

            return services;
        }

        public static IServiceCollection AddOtherRepositories(this IServiceCollection services)
        {
            services.AddScoped<IConcertRepository, ConcertRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITicketProviderRepository, TicketProviderRepository>();

            services.AddTransient<IMapper, Mapper>();
            services.RegisterDbMapping();

            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IPodRepository, PodRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IParserSettingRepository, ParserSettingRepository>();

            return services;
        }
    }
}
