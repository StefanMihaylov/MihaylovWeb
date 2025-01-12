using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusTasksWrapper
    {
        public IEnumerable<NexusTask> Items { get; set; }
    }
}
