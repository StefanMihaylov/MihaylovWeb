using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class StorageClass
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IDictionary<string, string> Parameters { get; set; }

        public string Provisioner { get; set; }
        
        public string ReclaimPolicy { get; set; }
    }
}
