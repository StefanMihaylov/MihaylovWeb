using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class AnswerType
    {
        public AnswerType()
        {
            Persons = new HashSet<Person>();
        }

        public int AnswerTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAsked { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
