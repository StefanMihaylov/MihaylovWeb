using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mihaylov.Api.Gear.Core.Domain.Entities;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IGearDbContext
{
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<InventoryItem> InventoryItems { get; set; }

    DbSet<Trip> Trips { get; set; }

    DbSet<GearNode> GearNodes { get; set; }


    DbSet<Category> Categories { get; set; }

    DbSet<Brand> Brands { get; set; }

    DbSet<Shop> Shops { get; set; }

    DbSet<Currency> Currencies { get; set; }

    DbSet<Group> Groups { get; set; }
}