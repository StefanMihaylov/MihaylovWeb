using System.Collections.Generic;
using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class PersistentVolumeClaim
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IDictionary<string, string> Labels { get; set; }

        public string Capacity { get; set; }

        public string VolumeName { get; set; }

        public string VolumeMode { get; set; }

        public string StorageClassName { get; set; }

        public string Namespace { get; set; }

        public string Status { get; set; }

        public string AccessMode { get; set; }
    }
}
