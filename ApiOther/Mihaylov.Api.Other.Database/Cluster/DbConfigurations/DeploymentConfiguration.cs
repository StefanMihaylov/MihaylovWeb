using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Cluster.Models;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
    {
        public void Configure(EntityTypeBuilder<Deployment> builder)
        {
            builder.HasKey(b => b.DeploymentId).HasName("DeploymentId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(20);

            builder.HasData(
                Enum.GetValues(typeof(DeploymentType))
                    .Cast<DeploymentType>()
                    .Select(e => new Deployment()
                    {
                        DeploymentId = (byte)e,
                        Name = e.ToString()
                    })
            );

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
