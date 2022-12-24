using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mihaylov.Common.Abstract.Databases.Interfaces
{
    public interface IAuditService
    {
        void ApplyAuditInformation(List<EntityEntry> entities);
    }
}