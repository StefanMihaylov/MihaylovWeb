using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Entities;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(x => x.InventoryItemId);
        builder.Property(x => x.InventoryItemId).ValueGeneratedOnAdd().UseIdentityColumn(1000, 1);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(3000);

        builder.Property(x => x.Price).IsRequired(false).HasPrecision(18, 2);
        builder.Property(x => x.PurchaseDate).IsRequired(false).HasColumnType("date");

        builder.Property(x => x.BrandId).IsRequired(false);
        builder.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.ShopId).IsRequired(false);
        builder.HasOne(x => x.Shop).WithMany().HasForeignKey(x => x.ShopId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.CategoryId).IsRequired(true);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.CurrencyId).IsRequired(false);
        builder.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.ItemStatusId).IsRequired(true);
        builder.HasOne(x => x.ItemStatus).WithMany().HasForeignKey(x => x.ItemStatusId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);


        builder.OwnsMany(x => x.KitContents, kit =>
        {
            kit.ToJson();
            kit.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
        });


        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.ItemStatusId);
    }
}
