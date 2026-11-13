using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NugetPackages
    {
        public string LocalPackagePrefix { get; set; }

        public IEnumerable<NexusNugetImage> Nugets { get; set; }
    }
}
