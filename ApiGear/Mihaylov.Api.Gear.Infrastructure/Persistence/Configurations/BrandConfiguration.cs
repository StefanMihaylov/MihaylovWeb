using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(x => x.BrandId);
        builder.Property(x => x.BrandId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(100);
    }
}
