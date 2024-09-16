using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(b => b.UnitId).HasName("PK_UnitId");

            builder.Property(t => t.UnitId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.Unit.NameMaxLength);
            builder.Property(c => c.ConversionRate).IsRequired(false);

            builder.HasIndex(t => t.Name).IsUnique();

            builder.Property(c => c.BaseUnitId).IsRequired(false);
            builder.HasOne(c => c.BaseUnit).WithMany().HasForeignKey(c => c.BaseUnitId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
