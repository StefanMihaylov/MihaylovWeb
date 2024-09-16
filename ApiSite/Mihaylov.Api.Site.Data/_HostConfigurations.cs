using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Helpers;
using Mihaylov.Api.Site.Data.Managers;
using Mihaylov.Api.Site.Data.Repositories;
using Mihaylov.Api.Site.Data.Writers;
using Mihaylov.Api.Site.Database;
using Mihaylov.Api.Site.DatabaseOld;
using Mihaylov.Api.Site.DatabaseOld.Database;
using Mihaylov.Common.Abstract.Databases;

namespace Mihaylov.Api.Site.Data
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddSiteDatabase(this IServiceCollection services, Action<ConnectionStringSettings> connectionString)
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            services.AddDbContext<SiteDbContext>(options =>
             {
                 options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
                 {
                     opt.MigrationsHistoryTable("__MigrationsHistory");

                 });
                 options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
             });

            var connectionStringSettings2 = new ConnectionStringSettings();
            connectionString(connectionStringSettings2);
            connectionStringSettings2.DatabaseName = "MihaylovDb_old";

            services.AddDbContext<CamContext>(options =>
            {
                options.UseSqlServer(connectionStringSettings2.GetConnectionString(), opt =>
                {
                    // opt.MigrationsHistoryTable("__MigrationsHistory", MihaylovOtherShowDbContext.SCHEMA_NAME);

                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });

            return services;
        }

        public static IServiceCollection AddSiteServices(this IServiceCollection services, Action<SiteCoreOptions> siteOption)
        {
            services.AddScoped<ICamRepository, CamRepository>();
            services.AddScoped<IMigrateService, MigrateService>();

            services.RegisterDbMapping();

            services.Configure<SiteCoreOptions>(siteOption);

            services.AddScoped<ILocationsRepository, LocationsRepository>();
            services.AddScoped<ILookupTablesRepository, LookupTablesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();

            services.AddScoped<ICollectionsManager, CollectionsManager>();
            services.AddScoped<IPersonsManager, PersonsManager>();
            services.AddScoped<IPhrasesManager, PhrasesManager>();

            services.AddScoped<ICountriesWriter, CountriesWriter>();
            services.AddScoped<IPersonsWriter, PersonsWriter>();
            services.AddScoped<IPhrasesWriter, PhrasesWriter>();

            services.AddScoped<ICsQueryWrapper, CsQueryWrapper>();
            services.AddScoped<ISiteHelper, SiteHelper>();

            return services;
        }
    }
}
