using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusImages
    {
        public IDictionary<string, IEnumerable<NexusImage>> Images { get; set; }
    }
}
