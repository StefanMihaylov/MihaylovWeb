using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NugetPackage
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public string Framework { get; set; }

        public IEnumerable<NugetPackageDependency> Dependencies { get; set; }
    }
}
