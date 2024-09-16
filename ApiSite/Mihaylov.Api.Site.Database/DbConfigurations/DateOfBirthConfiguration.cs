using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class DateOfBirthConfiguration : IEntityTypeConfiguration<DateOfBirth>
    {
        public void Configure(EntityTypeBuilder<DateOfBirth> builder)
        {
            builder.HasKey(b => b.DateOfBirthId).HasName("DateOfBirthId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(30);

            builder.HasData(
                Enum.GetValues(typeof(DateOfBirthType))
                    .Cast<DateOfBirthType>()
                    .Select(e => new DateOfBirth()
                    {
                        DateOfBirthId = (byte)e,
                        Name = e.ToString()
                    })
            );

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
