using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.HasKey(t => t.MediaFileId).HasName("PK_MediaFiles_MediaFileId");
            builder.Property(t => t.MediaFileId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(t => t.FileName).IsRequired().HasMaxLength(DTO.MediaFile.FileNameMaxLength);
            builder.Property(t => t.SizeInBytes).IsRequired();
            builder.Property(t => t.CreateDate).IsRequired().HasPrecision(3);
            builder.Property(t => t.CheckSum).IsRequired(false).HasMaxLength(DTO.MediaFile.ChecksumMaxLength);
            builder.Property(t => t.GroupId).IsRequired(false);

            builder.Property(t => t.ExtensionId).IsRequired();
            builder.HasOne(t => t.Extension).WithMany().IsRequired().HasForeignKey(t => t.ExtensionId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.SourceId).IsRequired();
            builder.HasOne(t => t.Source).WithMany().IsRequired().HasForeignKey(t => t.SourceId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.AccountId).IsRequired(false);
            builder.HasOne(t => t.Account).WithMany().IsRequired(false).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(t => t.CheckSum).HasFilter("[CheckSum] IS NOT NULL").IsUnique();
            builder.HasIndex(t => t.GroupId).HasFilter("[GroupId] IS NOT NULL");
        }
    }
}
