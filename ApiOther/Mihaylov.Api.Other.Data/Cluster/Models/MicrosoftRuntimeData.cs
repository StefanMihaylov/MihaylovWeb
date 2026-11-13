using System;
using System.Collections.Generic;
using Mihaylov.Api.Other.Data.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class MicrosoftRuntimeData
    {
        public string Runtime { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public long Size { get; set; }

        public int LayersCount { get; set; }

        public IEnumerable<ManifestLayer> Layers { get; set; }
    }
}
