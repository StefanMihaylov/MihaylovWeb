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
        public static IServiceCollection AddMapping(this IServiceCollection services, Assembly currentAssembly, params string[] assemblies)
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

        public static IServiceCollection MigrateDatabase<T>(this IServiceCollection serviceProvider, Action<DatabaseFacade> migrate) where T : DbContext
        {
            using var provider = serviceProvider.BuildServiceProvider();
            using var dbContect = provider.GetRequiredService<T>();

            migrate(dbContect.Database);

            return serviceProvider;
        }
    }
}
