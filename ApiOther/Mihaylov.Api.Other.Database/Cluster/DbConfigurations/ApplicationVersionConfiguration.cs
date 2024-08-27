using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class ApplicationVersionConfiguration : IEntityTypeConfiguration<ApplicationVersion>
    {
        public void Configure(EntityTypeBuilder<ApplicationVersion> builder)
        {
            builder.HasKey(b => b.VersionId).HasName("VersionId");

            builder.Property(c => c.Version).IsRequired(true).HasMaxLength(ModelConstants.AppVersionMaxLength);
            builder.Property(c => c.HelmVersion).IsRequired(false).HasMaxLength(ModelConstants.AppVersionMaxLength);
            builder.Property(c => c.HelmAppVersion).IsRequired(false).HasMaxLength(ModelConstants.AppVersionMaxLength);

            builder.Property(c => c.ReleaseDate).IsRequired().HasColumnType("Date");

            builder.Property(c => c.ApplicationId).IsRequired();
            builder.HasOne(c => c.Application).WithMany(a => a.Versions).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.EntityConfiguration();
        }
    }
}
