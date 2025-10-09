using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class ParserSettingModel
    {
        public int? Id { get; set; }

        public int? ApplicationId { get; set; }

        public string Name { get; set; }

        [Required]
        public VersionUrlType? VersionUrlType { get; set; }

        [Required]
        [StringLength(ModelConstants.ParserSelectorMaxLength, MinimumLength = 1)]
        public string VersionSelector { get; set; }

        [StringLength(ModelConstants.ParserComandsMaxLength)]
        public string VersionCommand { get; set; }

        public VersionUrlType? ReleaseDateUrlType { get; set; }

        [StringLength(ModelConstants.ParserSelectorMaxLength)]
        public string ReleaseDateSelector { get; set; }

        [StringLength(ModelConstants.ParserComandsMaxLength)]
        public string ReleaseDateCommand { get; set; }
    }
}
