using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class PersistentVolume
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IDictionary<string, string> Labels { get; set; }

        public string Capacity { get; set; }

        public string StorageClassName { get; set; }

        public string Namespace { get; set; }

        public string Status { get; set; }

        public string AccessMode { get; set; }

        public string Claim { get; set; }

        public Guid ClaimId { get; set; }

        public string Path { get; set; }

        public string ReclaimPolicy { get; set; }

        public string csiVolumeName { get; set; }

        public IDictionary<string, string> VolumeAttributes { get; set; }

        public string ImageName { get; set; }

        public string ImagePool { get; set; }
    }
}
