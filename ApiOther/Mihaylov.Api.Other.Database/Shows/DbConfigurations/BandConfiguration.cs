using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Show;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class BandConfiguration : IEntityTypeConfiguration<Band>
    {
        public void Configure(EntityTypeBuilder<Band> builder)
        {
            builder.HasKey(b => b.BandId).HasName("BandId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.BandNameMaxLength);
            builder.EntityConfiguration();

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
