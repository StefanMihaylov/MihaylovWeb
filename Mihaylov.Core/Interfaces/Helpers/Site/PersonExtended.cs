using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Models.Site
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