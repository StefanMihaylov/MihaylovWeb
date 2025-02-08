using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class KubeNamespace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IDictionary<string, string> Labels { get; set; }
    }
}
