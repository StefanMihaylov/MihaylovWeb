using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models
{
    public class ApplicationExtended : Application
    {
        public IEnumerable<Pod> Pods { get; set; }

        public IEnumerable<DeploymentFile> Files { get; set; }

        public AppVersion Version { get; set; }
    }
}
