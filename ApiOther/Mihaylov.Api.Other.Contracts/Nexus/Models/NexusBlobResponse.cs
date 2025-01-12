using System;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusBlobResponse
    {
        public string Name { get; set; }
        
        public string Type { get; set; }

        public bool Unavailable { get; set; }

        public long BlobCount { get; set; }

        public long TotalSizeInBytes { get; set; }

        public long AvailableSpaceInBytes { get; set; }
    }
}
