using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Mihaylov.Common;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Generic DbContext base that applies audit metadata from an IAuditService to tracked entities before persisting
/// changes.
/// </summary>
/// <typeparam name="TContext">The derived DbContext type used for configuration and dependency injection.</typeparam>
public abstract class BaseDbContext<TContext> : DbContext where TContext : DbContext
{
    private readonly IAuditService _auditService;

    /// <summary>
    /// Initializes a new BaseDbContext with the specified DbContext options and audit service.
    /// </summary>
    /// <param name="options">The DbContextOptions used to configure the context.</param>
    /// <param name="auditService">The audit service used to record audit entries for entity changes and operations.</param>
    public BaseDbContext(DbContextOptions<TContext> options, IAuditService auditService)
        : base(options)
    {
        _auditService = auditService;
    }

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">If true, AcceptAllChanges is invoked after the changes are successfully written to the database.</param>
    /// <returns>The number of state entries written to the underlying database.</returns>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyAuditInformation();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    /// Saves all changes made in the context to the database asynchronously after applying audit information.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">true to accept all changes in the context after they are saved to the database; otherwise, false.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation of the asynchronous save operation.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries
    /// written to the database.</returns>
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
