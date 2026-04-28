using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasKey(x => x.ShopId);
        builder.Property(x => x.ShopId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
    }
}
