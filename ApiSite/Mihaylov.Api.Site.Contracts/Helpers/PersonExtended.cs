using System.Collections.Generic;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Helpers
{
    public class PersonExtended : Person
    {
        public PersonExtended()
        {
        }

        public PersonExtended(Person person)
            // : base(person)
        {
        }

        public IEnumerable<UnitShort> AnswerUnits { get; set; }

        public IEnumerable<AccountStatus> AnswerTypes { get; set; }
    }
}
