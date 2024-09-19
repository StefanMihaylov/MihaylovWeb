using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.DAL.Repositories;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Abstract.Databases;

namespace Mihaylov.Api.Site.DAL
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

            return services;
        }

        public static IServiceCollection AddSiteRepositories(this IServiceCollection services)
        {
            services.RegisterDbMapping();

            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();

            return services;
        }
    }
}
