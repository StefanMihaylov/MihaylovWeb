using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class KubernetesSchedule
    {
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Schedule { get; set; }

        public bool? Paused { get; set; }

        public string CsiSnapshotTimeout { get; set; }

        public DateTime? LastBackup { get; set; }

        public SchedulePhaseType? Phase { get; set; }

        public IEnumerable<string> IncludedNamespaces { get; set; }

        public IEnumerable<string> ExcludedResources { get; set; }

        public IDictionary<string, string> MatchLabels { get; set; }

        public string ItemOperationTimeout { get; set; }

        public bool SnapshotMoveData { get; set; }

        public string StorageLocation { get; set; }

        public string Expiration { get; set; }        
    }
}
