using System;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Other.Database.Cluster.Models
{
    public class ApplicationVersion : Entity
    {
        public int VersionId { get; set; }

        public int ApplicationId { get; set; }

        public string Version { get; set; }

        public string HelmVersion { get; set; }

        public string HelmAppVersion { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Application Application { get; set; }
    }
}
