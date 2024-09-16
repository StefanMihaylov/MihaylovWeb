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
            builder.HasKey(t => t.PersonId).HasName("PK_Persons_PersonId");
            builder.Property(t => t.PersonId).ValueGeneratedOnAdd().IsRequired().UseIdentityColumn(1000, 1);

            builder.Property(t => t.Comments).IsRequired(false).HasMaxLength(DTO.Person.CommentsMaxLength);

            builder.Property(c => c.DateOfBirth).IsRequired(false).HasColumnType("Date");

            builder.EntityConfiguration();

            builder.Property(c => c.DateOfBirthId).IsRequired(false);
            builder.HasOne(c => c.DateOfBirthModel).WithMany().HasForeignKey(c => c.DateOfBirthId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.CountryId).IsRequired(false);
            builder.HasOne(t => t.Country).WithMany().IsRequired(false).HasForeignKey(t => t.CountryId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Location).WithOne(p => p.Person).IsRequired(false).HasForeignKey<PersonLocation>(t => t.PersonId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.Details).WithOne(p => p.Person).IsRequired(false).HasForeignKey<PersonDetail>(t => t.PersonId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.EthnicityId).IsRequired(false);
            builder.HasOne(t => t.Ethnicity).WithMany().IsRequired(false).HasForeignKey(t => t.EthnicityId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.OrientationId).IsRequired(false);
            builder.HasOne(t => t.Orientation).WithMany().IsRequired(false).HasForeignKey(t => t.OrientationId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
