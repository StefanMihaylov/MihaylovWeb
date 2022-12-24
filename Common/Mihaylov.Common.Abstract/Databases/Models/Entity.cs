using System;
using Mihaylov.Common.Abstract.Databases.Interfaces;

namespace Mihaylov.Common.Abstract.Databases.Models
{
    public abstract class Entity : IEntity
    {
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
