using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Models
{
    public class PersonExtended : Person
    {
        public PersonExtended()
        {
        }

        public PersonExtended(Person person)
            : base(person)
        {
        }

        public IEnumerable<Unit> AnswerUnits { get; set; }

        public IEnumerable<AnswerType> AnswerTypes { get; set; }
    }
}