using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonStatistics
    {
        public IEnumerable<PersonStatisticsPairGeneric<bool>> Answers { get; set; }

        public IEnumerable<PersonStatisticsPairGeneric<string>> AccountTypes { get; set; }

        public IEnumerable<PersonStatisticsPairGeneric<string>> States { get; set; }

        public decimal Average { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public int TotalPersonCount { get; set; }

        public int ActiveAccountCount { get; set; }
    }
}