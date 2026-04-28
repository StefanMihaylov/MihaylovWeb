using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.CurrencyId);
        builder.Property(x => x.CurrencyId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(3);
        builder.Property(x => x.Symbol).IsRequired(true).HasMaxLength(3);
        builder.Property(x => x.IsDefault).IsRequired(true).HasDefaultValue(false);
    }
}
