using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Web.Models.Site
{
    public class PersonGridModel
    {
        public PersonStatistics Statistics { get; set; }

        public IEnumerable<Person> Persons { get; set; }

        public string SystemUnit { get; set; }

        public IEnumerable<Phrase> Phrases { get; set; }
    }
}