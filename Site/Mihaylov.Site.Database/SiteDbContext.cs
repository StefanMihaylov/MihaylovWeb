using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Databases.Interfaces;
using Mihaylov.Site.Database.Models;
using Mihaylov.Site.Database.Models.DbConfigurations;

namespace Mihaylov.Site.Database
{
    public class SiteDbContext : DbContext
    {
        private readonly IAuditService _auditService;

        public SiteDbContext(DbContextOptions<SiteDbContext> options, IAuditService auditService)
            : base(options)
        {
            _auditService = auditService;
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Phrase> Phrases { get; set; }


        public DbSet<DateOfBirthModel> DateOfBirthTypes { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<EthnicityType> EthnicityTypes { get; set; }

        public DbSet<OrientationType> OrientationTypes { get; set; }

        public DbSet<AccountType> AccountTypes { get; set; }


        // public virtual DbSet<AnswerType> AnswerTypes { get; set; }

        // public virtual DbSet<UnitType> UnitTypes { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new PhraseConfiguration());

            modelBuilder.ApplyConfiguration(new DateOfBirthTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LookupTableConfiguration<AccountType>(25));
            modelBuilder.ApplyConfiguration(new LookupTableConfiguration<Country>(50));
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new LookupTableConfiguration<EthnicityType>(25));
            modelBuilder.ApplyConfiguration(new LookupTableConfiguration<OrientationType>(25));

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyAuditInformation()
        {
            var entities = this.ChangeTracker.Entries().ToList();
            this._auditService.ApplyAuditInformation(entities);
        }
    }
}
