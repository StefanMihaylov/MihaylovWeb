using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusImages
    {
        public IEnumerable<NexusDockerImage> Images { get; set; }

        public IEnumerable<MicrosoftRuntime> Runtimes { get; set; }
    }
}
