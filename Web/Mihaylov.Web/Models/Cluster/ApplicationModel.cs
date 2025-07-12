using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Cluster
{
    public class ApplicationViewModel
    {
        public ApplicationExtended Main { get; set; }

        public LastVersionModel LastVersion { get; set; }

        public VersionIconType Icon { get; set; }

        public bool IsNew => Icon == VersionIconType.New && LastVersion?.IsSuccessful == true;
    }
}
