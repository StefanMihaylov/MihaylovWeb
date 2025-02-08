using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusResponseWrapper
    {
        public IEnumerable<NexusResponse> Items { get; set; }

        public string ContinuationToken { get; set; }
    }
}
