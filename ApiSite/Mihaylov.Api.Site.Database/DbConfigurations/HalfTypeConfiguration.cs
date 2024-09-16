using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class HalfTypeConfiguration : IEntityTypeConfiguration<HalfType>
    {
        public void Configure(EntityTypeBuilder<HalfType> builder)
        {
            builder.HasKey(b => b.HalfTypeId).HasName("PK_HalfTypeId");

            builder.Property(t => t.HalfTypeId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.HalfType.NameMaxLength);
        }
    }
}
