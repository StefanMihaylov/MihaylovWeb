using Mihaylov.Api.Other.Contracts.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class AppVersionModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.AppVersionMaxLength, MinimumLength = 1)]
        public string Version { get; set; }

        [StringLength(ModelConstants.AppVersionMaxLength)]
        public string HelmVersion { get; set; }

        [StringLength(ModelConstants.AppVersionMaxLength)]
        public string HelmAppVersion { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int ApplicationId { get; set; }
    }
}
