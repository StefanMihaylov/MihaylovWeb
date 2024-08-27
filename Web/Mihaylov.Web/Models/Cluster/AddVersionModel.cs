using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Web.Models.Cluster
{
    public class AddVersionModel
    {
        [Required]
        public int? ApplicationId { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Version { get; set; }

        public string HelmVersion { get; set; }

        public string HelmAppVersion { get; set; }


        public IEnumerable<SelectListItem> Applications { get; set; }
    }
}
