using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(b => b.ApplicationId).HasName("ApplicationId");

            builder.Property(c => c.Order).IsRequired(false);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.AppNameMaxLength);
            builder.Property(c => c.ReleaseUrl).IsRequired().HasMaxLength(ModelConstants.AppUrlMaxLength);
            builder.Property(c => c.SiteUrl).IsRequired(false).HasMaxLength(ModelConstants.AppUrlMaxLength);
            builder.Property(c => c.ResourceUrl).IsRequired(false).HasMaxLength(ModelConstants.AppUrlMaxLength);
            builder.Property(c => c.GithubVersionUrl).IsRequired(false).HasMaxLength(ModelConstants.AppUrlMaxLength);
            builder.Property(c => c.Notes).IsRequired(false).HasMaxLength(ModelConstants.AppNotesMaxLength);

            builder.Property(c => c.DeploymentId).IsRequired().HasDefaultValue(1);
            builder.HasOne(c => c.Deployment).WithMany().HasForeignKey(c => c.DeploymentId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.EntityConfiguration();

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
