using System;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Common.Database.Models
{
    public abstract class Entity : IEntity
    {
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
