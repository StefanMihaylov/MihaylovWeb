using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class LearningSystemConfiguration : IEntityTypeConfiguration<LearningSystem>
    {
        public void Configure(EntityTypeBuilder<LearningSystem> builder)
        {
            builder.HasKey(t => t.LearningSystemId).HasName("PK_LearningSystems_LearningSystemId");
            builder.Property(t => t.LearningSystemId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(DTO.LearningSystem.NameMaxLength);

            builder.Property(t => t.LanguageId).IsRequired();
            builder.HasOne(t => t.Language).WithMany().IsRequired().HasForeignKey(c => c.LanguageId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
