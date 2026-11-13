using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

internal class DotnetReleasesResponse
{
    [JsonPropertyName("releases")]
    public List<DotnetRelease> Releases { get; set; }
}

internal class DotnetRelease
{
    [JsonPropertyName("release-version")]
    public string ReleaseVersion { get; set; }

    [JsonPropertyName("release-date")]
    public DateOnly ReleaseDate { get; set; }
}
