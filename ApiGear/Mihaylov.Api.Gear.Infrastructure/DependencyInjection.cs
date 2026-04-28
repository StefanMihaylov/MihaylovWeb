using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Infrastructure.Persistence;
using Mihaylov.Common;
using Mihaylov.Common.Database.Models;

namespace Mihaylov.Api.Gear.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGearDatabase(this IServiceCollection services,
        Action<ConnectionStringSettings> connectionString)
    {
        services.AddDatabase<MihaylovGearDbContext>(connectionString);

        services.AddScoped<IGearDbContext>(provider => provider.GetRequiredService<MihaylovGearDbContext>());

        return services;
    }
}
