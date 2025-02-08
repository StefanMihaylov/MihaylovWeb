using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public IEnumerable<NexusResponseAsset> Assets { get; set; }
    }
}
