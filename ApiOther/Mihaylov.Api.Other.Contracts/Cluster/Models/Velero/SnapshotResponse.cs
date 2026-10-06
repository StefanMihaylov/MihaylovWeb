using System.Collections.Generic;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class SnapshotResponse
    {
        public IEnumerable<KopiaSnapshot> OrphanedSnapshots { get; set; }

        public IEnumerable<string> OrphanedUploads { get; set; }

        public SnapshotStatistics Statistics { get; set; }
    }
}
