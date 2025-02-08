using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusTasksWrapper
    {
        public IEnumerable<NexusTask> Items { get; set; }
    }
}
