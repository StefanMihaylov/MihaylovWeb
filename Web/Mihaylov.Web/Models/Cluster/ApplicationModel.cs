using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Cluster
{
    public class ApplicationViewModel
    {
        public ApplicationExtended Main { get; set; }

        public LastVersionModel LastVersion { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool ParserSettingAvailable { get; set; }
    }
}
