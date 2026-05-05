using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class ConcertTypeConfiguration : IEntityTypeConfiguration<ConcertType>
    {
        public void Configure(EntityTypeBuilder<ConcertType> builder)
        {
            builder.HasKey(b => b.ConcertTypeId).HasName("ConcertTypeId");

            builder.Property(c => c.ConcertTypeId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.ConcertTypeMaxLength);

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
