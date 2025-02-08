using System;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Version
{
    public class LastVersionModel
    {
        public int ApplicationId { get; set; }

        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
