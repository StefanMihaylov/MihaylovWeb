using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Other.Database.Cluster.Models
{
    public class ParserSetting : Entity
    {
        public int ParserSettingId { get; set; }

        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public byte VersionUrlVersionId { get; set; }

        public VersionUrl VersionUrlVersion { get; set; }

        public string SelectorVersion { get; set; }

        public string CommandsVersion { get; set; }

        public byte? VersionUrlrReleaseId { get; set; }

        public VersionUrl VersionUrlrRelease { get; set; }

        public string SelectorRelease { get; set; }

        public string CommandsRelease { get; set; }
    }
}
