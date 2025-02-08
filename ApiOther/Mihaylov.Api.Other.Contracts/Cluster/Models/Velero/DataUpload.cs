using System;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class DataUpload
    {
        public string ClaimName { get; set; }

        public string VolumeName { get; set; }

        public string CephName { get; set; }

        public string StorageClassName { get; set; }

        public string Capacity { get; set; }

        public DataUploadPhaseType? Phase { get; set; }

        public long? BytesDone { get; set; }

        public long? TotalBytes { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? CompletionTimestamp { get; set; }
    }
}
