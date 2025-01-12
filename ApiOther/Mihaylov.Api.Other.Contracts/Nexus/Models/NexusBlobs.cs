using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
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
