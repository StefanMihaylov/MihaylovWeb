using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Other.Database.Shows.Models;
using Mihaylov.Api.Other.Contracts.Show;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(b => b.LocationId).HasName("LocationId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.LocationNameMaxLength);
            builder.EntityConfiguration();

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
