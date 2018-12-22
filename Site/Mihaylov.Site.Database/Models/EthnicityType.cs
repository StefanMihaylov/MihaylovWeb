using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class EthnicityType
    {
        public EthnicityType()
        {
            Persons = new HashSet<Person>();
        }

        public int EthnicityTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
