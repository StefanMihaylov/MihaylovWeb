using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(b => b.CountryId).HasName("PK_Countries_CountryId");

            builder.Property(t => t.CountryId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.Country.NameMaxLength);
            builder.Property(s => s.TwoLetterCode).IsRequired(false).HasMaxLength(2);
            builder.Property(s => s.ThreeLetterCode).IsRequired(false).HasMaxLength(3);
            builder.Property(s => s.AlternativeNames).IsRequired(false).HasMaxLength(DTO.Country.NameMaxLength);

            builder.HasIndex(b => b.Name).IsUnique();
            builder.HasIndex(b => b.TwoLetterCode).IsUnique().HasFilter(null);
            builder.HasIndex(b => b.ThreeLetterCode).IsUnique().HasFilter(null);
        }
    }
}
