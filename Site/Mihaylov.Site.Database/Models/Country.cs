using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class Country
    {
        public Country()
        {
            Persons = new HashSet<Person>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
