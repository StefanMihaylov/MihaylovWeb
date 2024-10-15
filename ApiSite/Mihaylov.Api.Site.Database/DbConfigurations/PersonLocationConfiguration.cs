using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class PersonLocationConfiguration : IEntityTypeConfiguration<PersonLocation>
    {
        public void Configure(EntityTypeBuilder<PersonLocation> builder)
        {
            builder.HasKey(t => t.PersonId).HasName("PK_PersonLocations_PersonId");
            builder.Property(t => t.PersonId).ValueGeneratedNever().IsRequired();

            builder.Property(t => t.Region).IsRequired(false).HasMaxLength(DTO.PersonLocation.RegionMaxLength);
            builder.Property(t => t.City).IsRequired(false).HasMaxLength(DTO.PersonLocation.CityMaxLength);
            builder.Property(t => t.Details).IsRequired(false).HasMaxLength(DTO.PersonLocation.DetailsMaxLength);

            builder.Property(t => t.CountryStateId).IsRequired(false);
            builder.HasOne(t => t.CountryState).WithMany().IsRequired(false).HasForeignKey(t => t.CountryStateId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
