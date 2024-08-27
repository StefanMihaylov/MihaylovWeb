using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Mihaylov.Api.Other.Client;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Cluster
{
    public class AddApplicationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ReleaseUrl { get; set; }

        public string ResourceUrl { get; set; }

        [Required]
        public DeploymentType? Deployment { get; set; }

        public string Notes { get; set; }


        public IEnumerable<SelectListItem> DeploymentTypes { get; set; }
    }
}
