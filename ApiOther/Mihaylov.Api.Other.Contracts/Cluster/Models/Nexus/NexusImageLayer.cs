namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusImageLayer
    {
        public long Size { get; set; }

        public string Digest { get; set; }

        public NexusImageLayer()
        {            
        }

        public NexusImageLayer(long size, string digest)
        {
            Size = size;
            Digest = digest;
        }
    }
}
