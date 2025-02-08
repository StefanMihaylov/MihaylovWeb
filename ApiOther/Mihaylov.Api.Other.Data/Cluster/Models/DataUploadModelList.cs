using System;
using k8s.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Models
{
    public class DataUploadModelList
    {
        public string ApiVersion { get; set; }

        public string Kind { get; set; }

        public DataUploadModelInternal[] Items { get; set; }

        public V1ListMeta Metadata { get; set; }
    }

    public class DataUploadModelInternal
    {
        public V1ObjectMeta Metadata { get; set; }

        public DataUploadModelSpec Spec { get; set; }

        public DataUploadModelStatus Status { get; set; }
    }

    public class DataUploadModelSpec
    {
        public string BackupStorageLocation { get; set; }

        public string OperationTimeout { get; set; }

        public string SnapshotType { get; set; }

        public string SourceNamespace { get; set; }

        public string SourcePVC { get; set; }
    }

    public class DataUploadModelStatus
    {
        public DateTime? CompletionTimestamp { get; set; }

        public string Phase { get; set; }

        public string SnapshotID { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public UploadProgress Progress { get; set; }
    }

    public class UploadProgress
    {
        public long? BytesDone { get; set; }

        public long? TotalBytes { get; set; }
    }
}
