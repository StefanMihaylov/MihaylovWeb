using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class CountryStateConfiguration : IEntityTypeConfiguration<CountryState>
    {
        public void Configure(EntityTypeBuilder<CountryState> builder)
        {
            builder.HasKey(b => b.StateId).HasName("PK_StateId");

            builder.Property(t => t.StateId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(DTO.CountryState.NameMaxLength);
            builder.Property(s => s.Code).IsRequired().HasMaxLength(2);

            builder.Property(c => c.CountryId).IsRequired();
            builder.HasOne(c => c.Country).WithMany(a => a.States).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
