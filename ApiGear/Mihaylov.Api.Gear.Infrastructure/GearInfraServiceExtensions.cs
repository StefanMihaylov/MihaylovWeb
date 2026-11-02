using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Infrastructure.Persistence;
using Mihaylov.Api.Gear.Infrastructure.Repositories;
using Mihaylov.Common;

namespace Mihaylov.Api.Gear.Infrastructure;

public static class GearInfraServiceExtensions
{
    public static IServiceCollection AddGearDatabase(this IServiceCollection services,
        Func<IServiceProvider, string?> userNameFunc, Action<ConnectionStringSettings> connectionString)
    {
        services.AddDatabase<MihaylovGearDbContext>(userNameFunc, connectionString);

        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<IGearNodeRepository, GearNodeRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        return services;
    }
}
