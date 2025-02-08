using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class KubernetesBackup
    {
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string ScheduleName { get; set; }

        public int? BackupItemOperationsAttempted { get; set; }

        public int? BackupItemOperationsCompleted { get; set; }

        public int? Errors { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? CompletionTimestamp { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public BackupPhaseType? Phase { get; set; }

        public int? ItemsBackedUp { get; set; }

        public int? TotalItems { get; set; }
    }
}
