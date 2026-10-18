using System.Collections.Generic;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class NexusManifestResponse
{
    public int SchemaVersion { get; set; }
    
    public string MediaType { get; set; }

    public IEnumerable<NexusManifest> Manifests { get; set; }

    public ManifestConfig Config { get; set; }

    public IEnumerable<ManifestLayer> Layers { get; set; }
}

public class NexusManifest
{
    public string MediaType { get; set; }

    public long Size { get; set; }

    public string Digest { get; set; }

    public NexusPlatform Platform { get; set; }
}

public class NexusPlatform
{
    public string Architecture { get; set; }

    public string Os { get; set; }

    public string Variant { get; set; }
}

public class ManifestConfig
{
    public string MediaType { get; set; }
    
    public long Size { get; set; }

    public string Digest { get; set; }
}

public class ManifestLayer
{
    public string MediaType { get; set; }

    public long Size { get; set; }

    public string Digest { get; set; }
}
