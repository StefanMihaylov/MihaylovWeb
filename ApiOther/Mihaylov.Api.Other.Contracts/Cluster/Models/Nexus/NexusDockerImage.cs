namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusDockerImage : NexusBaseImage
    {
        public int? LayersCount { get; set; }

        public long? AppSize { get; set; }

        public int? AppLayersCount { get; set; }


        public NexusDockerImage()
        {
        }

        public NexusDockerImage(NexusBaseImage image)
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

            LayersCount = null;
            AppSize = null;
            AppLayersCount = null;
        }
    }
}
