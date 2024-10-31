using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Common.Database.Services
{
    public class AuditService : IAuditService
    {
        private readonly ICurrentUserService _currentUser;

        public AuditService(ICurrentUserService currentUser)
        {
            this._currentUser = currentUser;
        }

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
}
