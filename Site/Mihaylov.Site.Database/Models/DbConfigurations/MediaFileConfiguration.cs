using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.Data).IsRequired();
            builder.Property(t => t.Extension).IsRequired().HasMaxLength(10);
            builder.Property(t => t.SizeInBytes).IsRequired();

            builder.HasOne(t => t.Source).WithMany().HasForeignKey(t => t.SourceId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Account).WithMany().HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
