using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class DateOfBirthTypeConfiguration : IEntityTypeConfiguration<DateOfBirthModel>
    {
        public void Configure(EntityTypeBuilder<DateOfBirthModel> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);//.HasConversion<int>();
            builder.Property(e => e.Name).HasMaxLength(25).IsRequired();

            builder.HasData(Enum.GetValues(typeof(DateOfBirthType)).Cast<DateOfBirthType>()
                          .Select(e => new DateOfBirthModel() { Id = (int)e, Name = e.ToString() }));
        }
    }
}
