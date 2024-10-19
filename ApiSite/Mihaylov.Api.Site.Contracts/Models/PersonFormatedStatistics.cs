using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonFormatedStatistics
    {
        public IEnumerable<PersonStatisticsPair> Data { get; set; }
    }
}
