using System;
using System.Collections.Generic;
using k8s.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Models
{
    public class ScheduleModelList
    {
        public string ApiVersion { get; set; }

        public string Kind { get; set; }

        public ScheduleModelInternal[] Items { get; set; }

        public V1ListMeta Metadata { get; set; }
    }

    public class ScheduleModelInternal
    {
        public V1ObjectMeta Metadata { get; set; }

        public ScheduleModelSpec Spec { get; set; }

        public ScheduleModelStatus Status { get; set; }
    }

    public class ScheduleModelSpec
    {
        public string Schedule { get; set; }

        public bool? Paused { get; set; }

        public ScheduleModelSpecTemplate Template { get; set; }
    }

    public class ScheduleModelSpecTemplate
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

    public class LabelSelector
    {
        public IDictionary<string, string> MatchLabels { get; set; }
    }

    public class ScheduleModelStatus
    {
        public DateTime? LastBackup { get; set; }

        public string Phase { get; set; }
    }
}
