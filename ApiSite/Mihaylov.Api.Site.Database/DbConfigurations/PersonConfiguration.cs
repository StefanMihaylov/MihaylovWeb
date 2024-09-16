using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(t => t.PersonId).HasName("PK_PersonId");
            builder.Property(t => t.PersonId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(t => t.FirstName).IsRequired(false).HasMaxLength(DTO.Person.NameMaxLength);
            builder.Property(t => t.MiddleName).IsRequired(false).HasMaxLength(DTO.Person.NameMaxLength);
            builder.Property(t => t.LastName).IsRequired(false).HasMaxLength(DTO.Person.NameMaxLength);
            builder.Property(t => t.OtherNames).IsRequired(false).HasMaxLength(DTO.Person.OtherNamesMaxLength);
            builder.Property(t => t.Comments).IsRequired(false).HasMaxLength(DTO.Person.CommentsMaxLength);
            builder.Property(t => t.Region).IsRequired(false).HasMaxLength(DTO.Person.RegionMaxLength);
            builder.Property(t => t.City).IsRequired(false).HasMaxLength(DTO.Person.CityMaxLength);

            builder.Property(c => c.DateOfBirth).IsRequired(false).HasColumnType("Date");

            builder.EntityConfiguration();

            builder.Property(c => c.DateOfBirthId).IsRequired(false);
            builder.HasOne(c => c.DateOfBirthModel).WithMany().HasForeignKey(c => c.DateOfBirthId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.CountryId).IsRequired(false);
            builder.HasOne(t => t.Country).WithMany().IsRequired(false).HasForeignKey(t => t.CountryId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.CountryStateId).IsRequired(false);
            builder.HasOne(t => t.CountryState).WithMany().IsRequired(false).HasForeignKey(t => t.CountryStateId).OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(t => t.EthnicityId).IsRequired(false);
            builder.HasOne(t => t.Ethnicity).WithMany().IsRequired(false).HasForeignKey(t => t.EthnicityId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.OrientationId).IsRequired(false);
            builder.HasOne(t => t.Orientation).WithMany().IsRequired(false).HasForeignKey(t => t.OrientationId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
