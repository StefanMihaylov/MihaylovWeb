using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Dictionary.Contracts.Repositories;
using Mihaylov.Api.Dictionary.DAL.Repositories;
using Mihaylov.Api.Dictionary.Database;
using Mihaylov.Common.Abstract.Databases;

namespace Mihaylov.Api.Dictionary.DAL
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddDictionaryDatabase(this IServiceCollection services, Action<ConnectionStringSettings> connectionString)
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            services.AddDbContext<DictionaryDbContext>(options =>
            {
                options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
                {
                    opt.MigrationsHistoryTable("__MigrationsHistory");

                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });

            return services;
        }

        public static IServiceCollection AddDictionaryRepositories(this IServiceCollection services)
        {
            services.RegisterDbMapping();

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IRecordRepository, RecordRepository>();

            return services;
        }
    }
}
