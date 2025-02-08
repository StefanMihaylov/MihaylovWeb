using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class CustomResource
    {
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Group { get; set; }

        public string Version { get; set; }

        public string Kind { get; set; }

        public string Plural { get; set; }
    }
}
