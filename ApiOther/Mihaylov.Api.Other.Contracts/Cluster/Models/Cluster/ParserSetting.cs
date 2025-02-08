namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster
{
    public class ParserSetting
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public VersionUrlType VersionUrlType { get; set; }

        public string VersionSelector { get; set; }

        public string VersionCommand { get; set; }

        public VersionUrlType? ReleaseDateUrlType { get; set; }

        public string ReleaseDateSelector { get; set; }

        public string ReleaseDateCommand { get; set; }
    }
}
