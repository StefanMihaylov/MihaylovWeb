using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class ParserSettingConfiguration : IEntityTypeConfiguration<ParserSetting>
    {
        public void Configure(EntityTypeBuilder<ParserSetting> builder)
        {
            builder.HasKey(b => b.ParserSettingId).HasName("ParserSettingId");

            builder.Property(c => c.Name).IsRequired(false).HasMaxLength(ModelConstants.ParserNameMaxLength);

            builder.Property(c => c.SelectorVersion).IsRequired().HasMaxLength(ModelConstants.ParserSelectorMaxLength);
            builder.Property(c => c.CommandsVersion).IsRequired().HasMaxLength(ModelConstants.ParserComandsMaxLength);

            builder.Property(c => c.SelectorRelease).IsRequired(false).HasMaxLength(ModelConstants.ParserSelectorMaxLength);
            builder.Property(c => c.CommandsRelease).IsRequired().HasMaxLength(ModelConstants.ParserComandsMaxLength);

            builder.Property(c => c.ApplicationId).IsRequired(false);
            builder.HasOne(c => c.Application).WithMany(a => a.ParserSettings).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.VersionUrlVersionId).IsRequired();
            builder.HasOne(c => c.VersionUrlVersion).WithMany(a => a.VersionSettings).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.VersionUrlrReleaseId).IsRequired(false);
            builder.HasOne(c => c.VersionUrlrRelease).WithMany(a => a.ReleaseSettings).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.EntityConfiguration();
        }
    }
}
