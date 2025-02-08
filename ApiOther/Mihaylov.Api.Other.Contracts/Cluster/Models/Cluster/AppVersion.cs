using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster
{
    public class AppVersion
    {
        public int Id { get; set; }

        public string Version { get; set; }

        public string HelmVersion { get; set; }

        public string HelmAppVersion { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
