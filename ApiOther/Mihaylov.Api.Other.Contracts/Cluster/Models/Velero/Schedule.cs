using System;
using System.Collections.Generic;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class Schedule
    {
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Cron { get; set; }

        public bool? Paused { get; set; }

        public string CsiSnapshotTimeout { get; set; }

        public DateTime? LastBackup { get; set; }

        public SchedulePhaseType? Phase { get; set; }

        public IEnumerable<string> IncludedNamespaces { get; set; }

        public IEnumerable<string> ExcludedResources { get; set; }

        public string ItemOperationTimeout { get; set; }

        public string MatchLabels { get; set; }

        public string Expiration { get; set; }          

        public bool SnapshotMoveData { get; set; }

        public string StorageLocation { get; set; }
                       

        public IEnumerable<Backup> Backups { get; set; }
    }
}
