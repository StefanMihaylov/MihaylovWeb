using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class ConcertConfiguration : IEntityTypeConfiguration<Models.Concert>
    {
        public void Configure(EntityTypeBuilder<Models.Concert> builder)
        {
            builder.HasKey(b => b.ConcertId).HasName("ConcertId");

            builder.Property(c => c.Date).IsRequired().HasColumnType("Date");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.ConcertNameMaxLength);
            builder.Property(c => c.Price).IsRequired().HasPrecision(18, 2);

            builder.Property(c => c.CurrencyId).IsRequired().HasDefaultValue(CurrencyType.BGN);
            builder.HasOne(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.LocationId).IsRequired();
            builder.HasOne(c => c.Location).WithMany(a => a.Concerts).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.TicketProviderId);
            builder.HasOne(c => c.TicketProvider).WithMany(a => a.Concerts).OnDelete(DeleteBehavior.NoAction);

            builder.EntityConfiguration();

            builder.HasMany(c => c.Bands).WithMany(b => b.Concerts).UsingEntity<ConcertBand>(opt =>
            {
                opt.HasOne(cb => cb.Concert).WithMany(c => c.ConcertBands).HasForeignKey(cb => cb.ConcertId).OnDelete(DeleteBehavior.NoAction);
                opt.HasOne(cb => cb.Band).WithMany(b => b.ConcertBands).HasForeignKey(cb => cb.BandId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
