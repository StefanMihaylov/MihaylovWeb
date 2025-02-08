using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class DataUploadModel
    {
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Backup { get; set; }

        public string BackupStorageLocation { get; set; }

        public string OperationTimeout { get; set; }

        public string SnapshotType { get; set; }

        public string SourceNamespace { get; set; }

        public string SourcePVC { get; set; }

        public DateTime? CompletionTimestamp { get; set; }

        public DataUploadPhaseType? Phase { get; set; }

        public string SnapshotID { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public long? BytesDone { get; set; }

        public long? TotalBytes { get; set; }
    }
}
