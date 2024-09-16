using System.Collections.Generic;

namespace Mihaylov.Api.Site.Database.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        public string Name { get; set; }

        public string TwoLetterCode { get; set; }

        public string ThreeLetterCode { get; set; }

        public string AlternativeNames { get; set; }

        public IEnumerable<CountryState> States { get; set; }
    }
}
