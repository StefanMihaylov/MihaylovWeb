using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(t => t.LanguageId).HasName("PK_Languages_LanguageId");
            builder.Property(t => t.LanguageId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(DTO.Language.NameMaxLength);
        }
    }
}
