using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonFormatedStatistics
    {
        public IEnumerable<PersonStatisticsPair> Data { get; set; }


        public IEnumerable<string> HeaderData { get; set; }

        public IEnumerable<IEnumerable<string>> GridData { get; set; }

        public IEnumerable<string> FooterData { get; set; }
    }
}
