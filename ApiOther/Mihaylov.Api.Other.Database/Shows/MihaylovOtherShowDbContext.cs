using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Other.Database.Shows.DbConfigurations;
using Mihaylov.Api.Other.Database.Shows.Models;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Api.Other.Database.Shows
{
    public class MihaylovOtherShowDbContext : BaseDbContext<MihaylovOtherShowDbContext>
    {
        public const string SCHEMA_NAME = "show";

        public MihaylovOtherShowDbContext(DbContextOptions<MihaylovOtherShowDbContext> options,
            IAuditService auditService) : base(options, auditService)
        {
        }

        public DbSet<Concert> Concerts { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<TicketProvider> TicketProviders { get; set; }

        public DbSet<Band> Bands { get; set; }

        public DbSet<ConcertBand> ConcertBands { get; set; }

        public DbSet<Currency> Currencies { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SCHEMA_NAME);

            builder.ApplyConfiguration(new ConcertConfiguration());
            builder.ApplyConfiguration(new LocationConfiguration());
            builder.ApplyConfiguration(new TicketProviderConfiguration());
            builder.ApplyConfiguration(new BandConfiguration());
            builder.ApplyConfiguration(new CurrencyConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
