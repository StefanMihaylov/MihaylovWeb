using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class MicrosoftRuntime
    {
        public string Runtime { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public long Size { get; set; }

        public int LayersCount { get; set; }

        public long? SizeIncrease { get; set; }

        public long? FootprintIncrease { get; set; }
    }
}
