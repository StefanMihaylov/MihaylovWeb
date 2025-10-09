using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class ApplicationModel
    {
        public int? Id { get; set; }

        public int? Order { get; set; }

        [Required]
        [StringLength(ModelConstants.AppNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(ModelConstants.AppUrlMaxLength, MinimumLength = 1)]
        public string ReleaseUrl { get; set; }

        [StringLength(ModelConstants.AppUrlMaxLength)]
        public string SiteUrl { get; set; }

        [StringLength(ModelConstants.AppUrlMaxLength)]
        public string ResourceUrl { get; set; }

        [StringLength(ModelConstants.AppUrlMaxLength)]
        public string GithubVersionUrl { get; set; }

        [Required]
        public DeploymentType? Deployment { get; set; }

        [StringLength(ModelConstants.AppNotesMaxLength)]
        public string Notes { get; set; }

        public int? ParserSettingId { get; set; }
    }
}
