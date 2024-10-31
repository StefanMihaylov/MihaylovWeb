using System.Threading.Tasks;
using System.Threading;
using Mihaylov.Common.Database.Interfaces;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class BaseDbContext<TContext> : DbContext where TContext : DbContext
    {
        private readonly IAuditService _auditService;

        public BaseDbContext(DbContextOptions<TContext> options, IAuditService auditService)
            : base(options)
        {
            _auditService = auditService;
        }

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

        private void ApplyAuditInformation()
        {
            var entities = this.ChangeTracker.Entries().ToList();
            this._auditService.ApplyAuditInformation(entities);
        }
    }
}
