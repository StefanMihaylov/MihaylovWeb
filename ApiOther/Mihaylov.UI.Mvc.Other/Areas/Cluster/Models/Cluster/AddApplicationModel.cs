using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Cluster.Models.Cluster
{
    public class AddApplicationModel
    {
        public int? Id { get; set; }

        public int? Order { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ReleaseUrl { get; set; }

        public string ResourceUrl { get; set; }

        public string SiteUrl { get; set; }

        public string GithubVersionUrl { get; set; }

        [Required]
        public DeploymentType? Deployment { get; set; }

        public string Notes { get; set; }

        public int? ParserSettingId { get; set; }

        public IEnumerable<SelectListItem> DeploymentTypes { get; set; }

        public IEnumerable<SelectListItem> ParserSettings { get; set; }
    }
}
