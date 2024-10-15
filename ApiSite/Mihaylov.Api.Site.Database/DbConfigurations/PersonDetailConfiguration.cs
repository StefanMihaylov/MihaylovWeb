using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    public class PersonDetailConfiguration : IEntityTypeConfiguration<PersonDetail>
    {
        public void Configure(EntityTypeBuilder<PersonDetail> builder)
        {
            builder.HasKey(t => t.PersonId).HasName("PK_PersonDetails_PersonId");
            builder.Property(t => t.PersonId).ValueGeneratedNever().IsRequired();

            builder.Property(t => t.FirstName).IsRequired(false).HasMaxLength(DTO.PersonDetail.NameMaxLength);
            builder.Property(t => t.MiddleName).IsRequired(false).HasMaxLength(DTO.PersonDetail.NameMaxLength);
            builder.Property(t => t.LastName).IsRequired(false).HasMaxLength(DTO.PersonDetail.NameMaxLength);
            builder.Property(t => t.OtherNames).IsRequired(false).HasMaxLength(DTO.PersonDetail.OtherNamesMaxLength);
        }
    }
}
