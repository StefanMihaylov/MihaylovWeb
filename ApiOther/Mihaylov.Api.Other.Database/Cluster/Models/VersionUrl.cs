using System.Collections.Generic;

namespace Mihaylov.Api.Other.Database.Cluster.Models
{
    public class VersionUrl
    {
        public byte VersionUrlId { get; set; } // 255

        public string Name { get; set; }

        public IEnumerable<ParserSetting> VersionSettings { get; set; }

        public IEnumerable<ParserSetting> ReleaseSettings { get; set; }


        public VersionUrl()
        {
            VersionSettings = new List<ParserSetting>();
            ReleaseSettings = new List<ParserSetting>();
        }
    }
}
