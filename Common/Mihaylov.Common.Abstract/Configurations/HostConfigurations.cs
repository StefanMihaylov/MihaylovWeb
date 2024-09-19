using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Abstract.Databases.Interfaces;
using Mihaylov.Common.Abstract.Databases.Services;
using Mihaylov.Common.Abstract.Infrastructure.Interfaces;
using Mihaylov.Common.Abstract.Infrastructure.Service;
using Mihaylov.Common.Mapping;

namespace Mihaylov.Common.Abstract
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddAutoMapping(this IServiceCollection services, Assembly currentAssembly, params string[] assemblies)
        {
            var autoMapper = new AutoMapperConfigurator(currentAssembly, assemblies);
            autoMapper.Execute();

            return services;
        }

        public static IServiceCollection AddCommon(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services
                .Add<ICurrentUserService, CurrentUserService>(lifetime)
                .Add<IAuditService, AuditService>(lifetime);

            return services;
        }

        public static IServiceCollection Add<TInterface, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime));

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
