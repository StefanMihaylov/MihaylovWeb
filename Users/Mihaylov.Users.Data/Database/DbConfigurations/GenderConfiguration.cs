using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Models.Enums;

namespace Mihaylov.Users.Data.Database.DbConfigurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasConversion<int>();
            builder.Property(e => e.Name).HasMaxLength(25).IsRequired();
            builder.HasData(Enum.GetValues(typeof(GenderType)).Cast<GenderType>()
                      .Select(e => new Gender() { Id = e, Name = e.ToString() }));
        }
    }
}
