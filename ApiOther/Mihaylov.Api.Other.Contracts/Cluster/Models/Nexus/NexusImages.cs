using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusImages
    {
        public IEnumerable<NexusImage> Images { get; set; }

        public IEnumerable<NexusImage> Nugets { get; set; }
    }
}
