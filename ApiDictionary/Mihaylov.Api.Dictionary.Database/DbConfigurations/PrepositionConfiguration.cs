using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class PrepositionConfiguration : IEntityTypeConfiguration<Preposition>
    {
        public void Configure(EntityTypeBuilder<Preposition> builder)
        {
            builder.HasKey(t => t.PrepositionId).HasName("PK_Prepositions_PrepositionTypeId");
            builder.Property(t => t.PrepositionId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Value).IsRequired().HasMaxLength(DTO.Preposition.ValueMaxLength);

            builder.Property(t => t.LanguageId).IsRequired();
            builder.HasOne(t => t.Language).WithMany().IsRequired().HasForeignKey(c => c.LanguageId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
