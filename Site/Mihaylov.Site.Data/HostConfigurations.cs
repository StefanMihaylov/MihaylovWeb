using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Abstract.Databases;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Repositories;
using Mihaylov.Site.Database;
using Mihaylov.Common.Abstract;

namespace Mihaylov.Site.Data
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddSiteRepositories(this IServiceCollection services, 
            Action<ConnectionStringSettings> connectionString, ServiceLifetime serviceLifetime)
        {
            services.AddSiteDatabase(connectionString, serviceLifetime);

            services.Add<ILocationsRepository, LocationsRepository>(serviceLifetime);
            services.Add<ILookupTablesRepository, LookupTablesRepository>(serviceLifetime);
            services.Add<IPersonsRepository, PersonsRepository>(serviceLifetime);
            services.Add<IPhrasesRepository, PhrasesRepository>(serviceLifetime);

            return services;
        }
    }
}
