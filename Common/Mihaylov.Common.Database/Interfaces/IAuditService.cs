using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mihaylov.Common.Database.Interfaces
{
    public interface IAuditService
    {
        void ApplyAuditInformation(List<EntityEntry> entities);
    }
}
