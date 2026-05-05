using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class BandConfiguration : IEntityTypeConfiguration<Band>
    {
        public void Configure(EntityTypeBuilder<Band> builder)
        {
            builder.HasKey(b => b.BandId).HasName("BandId");

            builder.Property(c => c.BandId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.BandNameMaxLength);
            builder.Property(c => c.CountryId).IsRequired(false);
            builder.EntityConfiguration();

            builder.HasOne(b => b.Country).WithMany().IsRequired(false).HasForeignKey(b => b.CountryId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
