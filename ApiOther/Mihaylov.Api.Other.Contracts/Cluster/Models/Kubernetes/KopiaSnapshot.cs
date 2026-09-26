using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class KopiaSnapshot
    {
        public string SnapshotId { get; set; }

        public string KopiaId { get; set; }

        public DateTime Date { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public long Files { get; set; }

        public long Dirs { get; set; }

        public long NumFailed { get; set; }

        public string LocationName { get; set; }

        public string VolumeNamespace { get; set; }
    }
}
