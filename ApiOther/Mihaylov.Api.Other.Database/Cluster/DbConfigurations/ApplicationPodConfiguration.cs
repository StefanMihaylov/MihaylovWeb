using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class ApplicationPodConfiguration : IEntityTypeConfiguration<ApplicationPod>
    {
        public void Configure(EntityTypeBuilder<ApplicationPod> builder)
        {
            builder.HasKey(b => b.ApplicationPodId).HasName("ApplicationPodId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.AppPodNameMaxLength);

            builder.Property(c => c.ApplicationId).IsRequired();
            builder.HasOne(c => c.Application).WithMany(a => a.Pods).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
