using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Databases;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Repositories;
using Mihaylov.Site.Database;

namespace Mihaylov.Site.Data
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddSiteRepositories(this IServiceCollection services, Action<ConnectionStringSettings> connectionString)
        {
            services.AddSiteDatabase(connectionString);

            services.AddScoped<ILocationsRepository, LocationsRepository>();
            services.AddScoped<ILookupTablesRepository, LookupTablesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<IPhrasesRepository, PhrasesRepository>();

            return services;
        }
    }
}
