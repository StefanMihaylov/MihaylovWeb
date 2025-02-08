using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NexusBlobs
    {
        public IEnumerable<NexusBlobResponse> Blobs { get; set; }


        public NexusBlobs(IEnumerable<NexusBlobResponse> blobs)
        {
            Blobs = blobs;
        }
    }
}
