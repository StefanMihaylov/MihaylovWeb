using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class MediaFileExtensionConfiguration : IEntityTypeConfiguration<MediaFileExtension>
    {
        public void Configure(EntityTypeBuilder<MediaFileExtension> builder)
        {
            builder.HasKey(b => b.ExtensionId).HasName("PK_ExtensionId");

            builder.Property(t => t.ExtensionId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.MediaFileExtension.NameMaxLength);
            builder.Property(c => c.IsImage).IsRequired();
        }
    }
}
