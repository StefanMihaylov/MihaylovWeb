using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusImage
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime? LastModified { get; set; }

        public bool IsLocked { get; set; }
    }
}
