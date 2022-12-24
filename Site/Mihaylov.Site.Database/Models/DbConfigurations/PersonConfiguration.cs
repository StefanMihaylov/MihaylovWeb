using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Common.Abstract.Databases.DbConfigurations;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.FirstName).HasMaxLength(50);
            builder.Property(t => t.MiddleName).HasMaxLength(50);
            builder.Property(t => t.LastName).HasMaxLength(50);
            builder.Property(t => t.OtherNames).HasMaxLength(100);
            builder.Property(t => t.Comments).HasMaxLength(500);

            builder.HasOne(t => t.DateOfBirthModel).WithMany().HasForeignKey(t => t.DateOfBirthType).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Country).WithMany().HasForeignKey(t => t.CountryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.State).WithMany().HasForeignKey(t => t.StateId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.EthnicityType).WithMany().HasForeignKey(t => t.EthnicityTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.OrientationType).WithMany().HasForeignKey(t => t.OrientationTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
