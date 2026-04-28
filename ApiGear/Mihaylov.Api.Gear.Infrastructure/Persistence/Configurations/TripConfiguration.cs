using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Entities;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(x => x.TripId);
        builder.Property(x => x.TripId).ValueGeneratedOnAdd().UseIdentityColumn(100, 1);

        builder.Property(x => x.Title).IsRequired(true).HasMaxLength(200);
        builder.Property(x => x.Year).IsRequired(true);
        builder.Property(x => x.Notes).IsRequired(false).HasMaxLength(2000);
        builder.Property(x => x.CreatedAt).IsRequired(true);
    }
}
