using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class MediaFileSourceConfiguration : IEntityTypeConfiguration<MediaFileSource>
    {
        public void Configure(EntityTypeBuilder<MediaFileSource> builder)
        {
            builder.HasKey(b => b.SourceId).HasName("PK_MediaFileSource_SourceId");

            builder.Property(t => t.SourceId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.MediaFileSource.NameMaxLength);
        }
    }
}
