using System;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Common.Database.Models
{
    public abstract class DeletableEntity : Entity, IDeletableEntity
    {
        public DateTime? DeletedOn { get; set; }

        public string DeletedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
