using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mihaylov.Common;

/// <summary>
/// Extension methods for IServiceCollection to register database-related services, add and configure a DbContext for
/// SQL Server, and execute database migrations.
/// </summary>
public static class DatabaseServiceExtensions
{
    /// <summary>
    /// Registers the current-user service and the audit service/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="userNameFunc">
    /// Returns the user name to audit with, resolved from the service provider on every call
    /// (for example from <c>IHttpContextAccessor</c>). A <c>null</c> result is audited as <c>"System"</c>.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance, so calls can be chained.</returns>
    /// <remarks>
    /// The services are added with <c>TryAddScoped</c>, so the first call wins: a delegate passed by a later call is ignored.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="userNameFunc"/> is <c>null</c>.</exception>
    public static IServiceCollection SetDatabase(this IServiceCollection services, Func<IServiceProvider, string> userNameFunc)
    {
        ArgumentNullException.ThrowIfNull(userNameFunc);

        services.TryAddScoped<ICurrentUserService>(sp => new DelegateCurrentUserService(() => userNameFunc(sp)));
        services.TryAddScoped<IAuditService, AuditService>();

        return services;
    }

    /// <summary>
    /// Adds and configures a DbContext of type TContext to the service collection using SQL Server and registers
    /// database-related services.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to configure and register.</typeparam>
    /// <param name="services">The service collection to which the DbContext and related database services are added.</param>
    /// <param name="connectionString">An action that configures ConnectionStringSettings used to build the SQL Server connection string.</param>
    /// <param name="schema">The schema name for the migrations history table; if null, the default schema is used.</param>
    /// <param name="userNameFunc">The audit user name delegate, see <see cref="SetDatabase"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="userNameFunc"/> or <paramref name="connectionString"/> is <c>null</c>.</exception>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services,
        Func<IServiceProvider, string> userNameFunc, Action<ConnectionStringSettings> connectionString, string schema = null) 
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        var connectionStringSettings = new ConnectionStringSettings();
        connectionString(connectionStringSettings);

        services.SetDatabase(userNameFunc);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(connectionStringSettings.GetConnectionString(), opt =>
            {
                opt.MigrationsHistoryTable("__MigrationsHistory", schema);

            });
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
        });

        return services;
    }

    /// <summary>
    /// Performs database initialization by resolving a DbContext of type T and invoking the provided migrate action
    /// when enabled.
    /// </summary>
    /// <typeparam name="T">The DbContext type used to resolve an instance from the service provider.</typeparam>
    /// <param name="serviceProvider">The service collection used to build a temporary ServiceProvider to resolve the DbContext.</param>
    /// <param name="enable">If true, the migrate action is invoked; otherwise no migration is performed.</param>
    /// <returns>The original IServiceCollection to allow method chaining.</returns>
    public static IServiceCollection MigrateDatabase<T>(this IServiceCollection serviceProvider, bool enable = true) where T : DbContext
    {
        if (enable)
        {
            using var provider = serviceProvider.BuildServiceProvider();
            using var dbContext = provider.GetRequiredService<T>();

            dbContext.Database.Migrate();
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
