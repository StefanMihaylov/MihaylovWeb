using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class EthnicityConfiguration : IEntityTypeConfiguration<Ethnicity>
    {
        public void Configure(EntityTypeBuilder<Ethnicity> builder)
        {
            builder.HasKey(b => b.EthnicityId).HasName("PK_Ethnicities_EthnicityId");

            builder.Property(t => t.EthnicityId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.Ethnicity.NameMaxLength);
            builder.Property(c => c.OtherNames).IsRequired(false).HasMaxLength(DTO.Ethnicity.OtherNameMaxLength);
        }
    }
}
