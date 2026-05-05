using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations;

internal class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(b => b.CountryId).HasName("CountryId");

        builder.Property(c => c.CountryId).IsRequired().ValueGeneratedOnAdd();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.CountryNameMaxLength);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(3);

        builder.HasIndex(b => b.Name).IsUnique();
    }
}
