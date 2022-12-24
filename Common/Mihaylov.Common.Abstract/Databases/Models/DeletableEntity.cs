using System;
using Mihaylov.Common.Abstract.Databases.Interfaces;

namespace Mihaylov.Common.Abstract.Databases.Models
{
    public abstract class DeletableEntity : Entity, IDeletableEntity
    {
        public DateTime? DeletedOn { get; set; }

        public string DeletedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
