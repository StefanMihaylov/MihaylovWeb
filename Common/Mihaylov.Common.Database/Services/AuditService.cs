using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mihaylov.Common;

/// <summary>
/// Applies audit metadata to EntityEntry instances by setting CreatedOn/CreatedBy for additions, ModifiedOn/ModifiedBy
/// for modifications, and converting deletions of IDeletableEntity into soft deletes (DeletedOn, DeletedBy, IsDeleted).
/// </summary>
public class AuditService : IAuditService
{
    private readonly ICurrentUserService _currentUser;

    /// <summary>
    /// Initializes a new instance of AuditService with the provided current user service.
    /// </summary>
    /// <param name="currentUser">Service that provides information about the current user.</param>
    public AuditService(ICurrentUserService currentUser)
    {
        this._currentUser = currentUser;
    }

    /// <summary>
    /// Apply audit information to the specified EntityEntry objects: set CreatedOn/CreatedBy for added entities,
    /// ModifiedOn/ModifiedBy for modified entities, and for IDeletableEntity mark deleted entities with DeletedOn,
    /// DeletedBy and IsDeleted while converting delete operations to modifications.
    /// </summary>
    public void ApplyAuditInformation(List<EntityEntry> entities)
    {
        var userName = this._currentUser.GetUserName();

        entities.ForEach(entry =>
        {
            if (entry.Entity is IDeletableEntity deletableEntity)
            {
                if (entry.State == EntityState.Deleted)
                {
                    deletableEntity.DeletedOn = DateTime.UtcNow;
                    deletableEntity.DeletedBy = userName;
                    deletableEntity.IsDeleted = true;

                    entry.State = EntityState.Modified;

                    return;
                }
            }

            if (entry.Entity is IEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entity.CreatedOn == DateTime.MinValue)
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                    }

                    if (string.IsNullOrEmpty(entity.CreatedBy))
                    {
                        entity.CreatedBy = userName;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                    entity.ModifiedBy = userName;
                }
            }
        });
    }
}
