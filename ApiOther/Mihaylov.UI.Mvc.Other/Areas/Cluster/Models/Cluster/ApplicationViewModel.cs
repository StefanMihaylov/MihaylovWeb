using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Cluster.Models.Cluster
{
    public class ApplicationViewModel
    {
        public ApplicationExtended Main { get; set; }

        public LastVersionModel LastVersion { get; set; }

        public VersionIconType Icon { get; set; }

        public bool IsNew => Icon == VersionIconType.New && LastVersion?.IsSuccessful == true;
    }

    public enum VersionIconType
    {
        None = 1,
        Unknown,
        Error,
        OK,
        New,
    }
}
