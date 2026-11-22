using System.Collections.Generic;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusNugetImage : NexusBaseImage
    {
        public IEnumerable<DependencyNodeFlat> Dependencies { get; set; }


        public NexusNugetImage()
        {
        }

        public NexusNugetImage(NexusBaseImage image)
        {
            Id = image.Id;
            Name = image.Name;
            Version = image.Version;
            LastModified = image.LastModified;
            Sha256 = image.Sha256;
            Runtime = image.Runtime;
            Size = image.Size;

            IsLocked = image.IsLocked;
            IsFirst = image.IsFirst;
        }
    }
}
