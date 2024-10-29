using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasKey(t => t.LevelId).HasName("PK_Levels_LevelId");
            builder.Property(t => t.LevelId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(DTO.Level.NameMaxLength);
            builder.Property(e => e.Descrition).IsRequired(false).HasMaxLength(DTO.Level.DescritionMaxLength);
        }
    }
}
