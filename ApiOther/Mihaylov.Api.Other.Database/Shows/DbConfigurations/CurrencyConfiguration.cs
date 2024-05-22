using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(b => b.CurrencyId).HasName("CurrencyId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(3);

            builder.HasData(
                Enum.GetValues(typeof(CurrencyType))
                    .Cast<CurrencyType>()
                    .Select(e => new Currency()
                    {
                        CurrencyId = (byte)e,
                        Name = e.ToString()
                    })
            );

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
