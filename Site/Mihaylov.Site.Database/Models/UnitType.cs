using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class UnitType
    {
        public UnitType()
        {
            Persons = new HashSet<Person>();
        }

        public int UnitTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ConversionRate { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
