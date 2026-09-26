namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class SnapshotStatistics
    {
        public int TotalUploadCount { get; set; }

        public int TotalSnapshotCount { get; set; }

        public int OrchanedSnapshotCount { get; set; }
    }
}
