using System;

namespace Mihaylov.Common.Databases.Models
{
    public interface IDeletableEntity: IEntity
    {
        DateTime? DeletedOn { get; set; }

        string DeletedBy { get; set; }

        bool IsDeleted { get; set; }
    }
}
