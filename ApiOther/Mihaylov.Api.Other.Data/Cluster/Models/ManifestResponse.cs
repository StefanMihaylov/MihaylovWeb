using System.Collections.Generic;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class ManifestResponse
{
    public int SchemaVersion { get; set; }
    
    public string MediaType { get; set; }

    public ManifestConfig Config { get; set; }

    public IEnumerable<ManifestLayer> Layers { get; set; }
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
