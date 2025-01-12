using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusResponseWrapper
    {
        public IEnumerable<NexusResponse> Items { get; set; }

        public string ContinuationToken { get; set; }
    }
}
