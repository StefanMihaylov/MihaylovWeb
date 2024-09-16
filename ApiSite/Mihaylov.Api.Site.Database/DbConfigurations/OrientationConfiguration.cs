using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class OrientationConfiguration : IEntityTypeConfiguration<Orientation>
    {
        public void Configure(EntityTypeBuilder<Orientation> builder)
        {
            builder.HasKey(b => b.OrientationId).HasName("PK_Orientations_OrientationId");

            builder.Property(t => t.OrientationId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.Orientation.NameMaxLength);
        }
    }
}
