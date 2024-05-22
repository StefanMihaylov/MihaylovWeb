using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Other.Database.Shows.DbConfigurations;
using Mihaylov.Api.Other.Database.Shows.Models;
using Mihaylov.Common.Abstract.Databases.Interfaces;

namespace Mihaylov.Api.Other.Database.Shows
{
    public class MihaylovOtherShowDbContext : DbContext
    {
        public const string SCHEMA_NAME = "show";

        private readonly IAuditService _auditService;

        public MihaylovOtherShowDbContext(DbContextOptions<MihaylovOtherShowDbContext> options,
            IAuditService auditService) : base(options)
        {
            this._auditService = auditService;
        }

        public DbSet<Concert> Concerts { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<TicketProvider> TicketProviders { get; set; }

        public DbSet<Band> Bands { get; set; }

        public DbSet<ConcertBand> ConcertBands { get; set; }

        public DbSet<Currency> Currencies { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

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

        private void ApplyAuditInformation()
        {
            var entities = ChangeTracker.Entries().ToList();

            _auditService.ApplyAuditInformation(entities);
        }
    }
}
