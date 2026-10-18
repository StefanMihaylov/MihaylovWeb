using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class NexusComponentsResponse
{
    public IEnumerable<NexusResponseItem> Items { get; set; }

    public string ContinuationToken { get; set; }
}

public class NexusResponseItem
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Version { get; set; }

    public IEnumerable<NexusResponseAsset> Assets { get; set; }
}

public class NexusResponseAsset
{
    // public string Id { get; set; }

    public string DownloadUrl { get; set; }

    public long FileSize { get; set; }

    public DateTime LastModified { get; set; }

    public Dictionary<string, string> Checksum { get; set; }

    public NexusResponseDocker Docker { get; set; }
}

public class NexusResponseDocker
{
    public IEnumerable<string> Env { get; set; }
}
