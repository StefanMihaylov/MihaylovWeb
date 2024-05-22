using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.FirstName).HasMaxLength(50);
            builder.Property(t => t.MiddleName).HasMaxLength(50);
            builder.Property(t => t.LastName).HasMaxLength(50);
            builder.Property(t => t.OtherNames).HasMaxLength(100);
            builder.Property(t => t.Comments).HasMaxLength(500);
            builder.EntityConfiguration();

            builder.HasOne(t => t.DateOfBirthModel).WithMany().HasForeignKey(t => t.DateOfBirthType).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Country).WithMany().HasForeignKey(t => t.CountryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.State).WithMany().HasForeignKey(t => t.StateId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.EthnicityType).WithMany().HasForeignKey(t => t.EthnicityTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.OrientationType).WithMany().HasForeignKey(t => t.OrientationTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
