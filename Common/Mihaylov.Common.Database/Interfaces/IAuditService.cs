using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mihaylov.Common;

/// <summary>
/// Applies audit metadata (for example, created and modified timestamps and user identifiers) to a set of EntityEntry
/// instances prior to persistence.
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Applies audit metadata (creation and modification timestamps and user identifiers) to the provided entity
    /// entries.
    /// </summary>
    /// <param name="entities">A list of EntityEntry instances representing tracked entities to update with audit metadata; handles entries in
    /// the Added and Modified states.</param>
    void ApplyAuditInformation(List<EntityEntry> entities);
}
