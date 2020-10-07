using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Databases.Interfaces;
using Mihaylov.Users.Data.Database.DbConfigurations;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Database
{
    public class MihaylovUsersDbContext : IdentityDbContext<User>
    {
        private readonly IAuditService _auditService;

        public MihaylovUsersDbContext(DbContextOptions<MihaylovUsersDbContext> options, IAuditService auditService)
            : base(options)
        {
            this._auditService = auditService;
        }

        public DbSet<Gender> Genders { get; set; }


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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new GenderConfiguration());

            base.OnModelCreating(builder);
        }

        private void ApplyAuditInformation()
        {
            var entities = this.ChangeTracker.Entries().ToList();

            this._auditService.ApplyAuditInformation(entities);
        }
    }
}
