using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster
{
    public class VersionHistory
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
