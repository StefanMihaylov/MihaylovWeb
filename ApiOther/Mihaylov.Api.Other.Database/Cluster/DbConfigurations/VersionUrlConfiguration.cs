using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;
using Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Database.Cluster.DbConfigurations
{
    internal class VersionUrlConfiguration : IEntityTypeConfiguration<VersionUrl>
    {
        public void Configure(EntityTypeBuilder<VersionUrl> builder)
        {
            builder.HasKey(b => b.VersionUrlId).HasName("VersionUrlId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(20);

            builder.HasData(
                Enum.GetValues(typeof(VersionUrlType))
                    .Cast<VersionUrlType>()
                    .Select(e => new VersionUrl()
                    {
                        VersionUrlId = (byte)e,
                        Name = e.ToString()
                    })
            );

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
