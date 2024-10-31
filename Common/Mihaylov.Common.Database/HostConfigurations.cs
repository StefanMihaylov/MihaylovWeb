using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Common.Database.Models;
using Mihaylov.Common.Database.Services;

namespace Mihaylov.Common
{
    public static class _HostConfigurations
    {
        public static IServiceCollection SetDatabase(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddScoped<ICurrentUserService, CurrentUserService>();
            services.TryAddScoped<IAuditService, AuditService>();

            return services;
        }

        public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, 
            Action<ConnectionStringSettings> connectionString) where TContext : DbContext
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            services.SetDatabase();

            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
                {
                    opt.MigrationsHistoryTable("__MigrationsHistory");

                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });

            return services;
        }

        public static IServiceCollection MigrateDatabase<T>(this IServiceCollection serviceProvider, Action<DatabaseFacade> migrate, bool enable = true) where T : DbContext
        {
            if (enable)
            {
                using var provider = serviceProvider.BuildServiceProvider();
                using var dbContext = provider.GetRequiredService<T>();

                migrate(dbContext.Database);
            }

            return serviceProvider;
        }

        //public static IServiceCollection SeedDatabase<T>(this IServiceCollection serviceProvider, string tableName, Action<T> seed) where T : DbContext
        //{
        //    using var provider = serviceProvider.BuildServiceProvider();
        //    using var dbContext = provider.GetRequiredService<T>();

        //    using (var transaction = dbContext.Database.BeginTransaction())
        //    {
        //        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} ON;");

        //        seed(dbContext);

        //        dbContext.SaveChanges();
        //        transaction.Commit();
        //    }

        //    return serviceProvider;
        //}
    }
}
