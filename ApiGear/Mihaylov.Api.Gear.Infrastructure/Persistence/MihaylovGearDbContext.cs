using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Domain.Entities;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Core.Domain.Lookups;
using Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence;

public class MihaylovGearDbContext : BaseDbContext<MihaylovGearDbContext>, IGearDbContext
{
    public MihaylovGearDbContext(DbContextOptions<MihaylovGearDbContext> options, IAuditService auditService)
        : base(options, auditService)
    {
    }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    public DbSet<Trip> Trips { get; set; }

    public DbSet<GearNode> GearNodes { get; set; }


    public DbSet<Category> Categories { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Shop> Shops { get; set; }

    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Group> Groups { get; set; }


    public DbSet<ItemStatusDb> ItemStatuses { get; set; }

    public DbSet<NodeTypeDb> NodeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new InventoryItemConfiguration());
        builder.ApplyConfiguration(new TripConfiguration());
        builder.ApplyConfiguration(new GearNodeConfiguration());

        builder.ApplyConfiguration(new BrandConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new CurrencyConfiguration());
        builder.ApplyConfiguration(new GroupConfiguration());
        builder.ApplyConfiguration(new ShopConfiguration());

        builder.ApplyConfiguration(new ItemStatusConfiguration());
        builder.ApplyConfiguration(new NodeTypeConfiguration());

        base.OnModelCreating(builder);
    }
}
