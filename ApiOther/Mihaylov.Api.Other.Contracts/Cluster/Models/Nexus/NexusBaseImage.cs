using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusBaseImage
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime? LastModified { get; set; }

        public string Sha256 { get; set; }

        public string Runtime { get; set; }

        public long Size { get; set; }


        public bool IsLocked { get; set; }

        public bool IsFirst { get; set; }
    }
}
