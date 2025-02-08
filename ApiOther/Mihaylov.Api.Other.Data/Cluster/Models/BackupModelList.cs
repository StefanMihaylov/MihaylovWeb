using System;
using System.Collections.Generic;
using k8s.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Models
{
    public class BackupModelList
    {
        public string ApiVersion { get; set; }

        public string Kind { get; set; }

        public BackupModelInternal[] Items { get; set; }

        public V1ListMeta Metadata { get; set; }
    }

    public class BackupModelInternal
    {
        public V1ObjectMeta Metadata { get; set; }

        public BackupModelSpec Spec { get; set; }

        public BackupModelStatus Status { get; set; }
    }

    public class BackupModelSpec
    {
        public string CsiSnapshotTimeout { get; set; }

        public IEnumerable<string> IncludedNamespaces { get; set; }

        public IEnumerable<string> ExcludedResources { get; set; }

        public string ItemOperationTimeout { get; set; }

        public LabelSelector LabelSelector { get; set; }

        public bool SnapshotMoveData { get; set; }

        public string StorageLocation { get; set; }

        public string Ttl { get; set; }
    }

    public class BackupModelStatus
    {
        public int? BackupItemOperationsAttempted { get; set; }

        public int? BackupItemOperationsCompleted { get; set; }

        public int? Errors { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? CompletionTimestamp { get; set; }

        public DateTime? Expiration { get; set; }

        public string Phase { get; set; }

        public BackupProgress Progress { get; set; }
    }

    public class BackupProgress
    {
        public int ItemsBackedUp { get; set; }

        public int TotalItems { get; set; }
    }
}
