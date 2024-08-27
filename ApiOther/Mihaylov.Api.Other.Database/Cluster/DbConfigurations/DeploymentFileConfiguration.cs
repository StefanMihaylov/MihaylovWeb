using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class DeploymentFileConfiguration : IEntityTypeConfiguration<DeploymentFile>
    {
        public void Configure(EntityTypeBuilder<DeploymentFile> builder)
        {
            builder.HasKey(b => b.FileId).HasName("FileId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.AppFileNameMaxLength);

            builder.Property(c => c.ApplicationId).IsRequired();
            builder.HasOne(c => c.Application).WithMany(a => a.Files).IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
