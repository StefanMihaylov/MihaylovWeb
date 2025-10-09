using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Cluster
{
    public class AddParserSettingModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int? ApplicationId { get; set; }

        [Required]
        public VersionUrlType? VersionUrlType { get; set; }

        [Required]
        public string VersionSelector { get; set; }

        public string VersionCommand { get; set; }

        public VersionUrlType? ReleaseDateUrlType { get; set; }

        public string ReleaseDateSelector { get; set; }

        public string ReleaseDateCommand { get; set; }


        public IEnumerable<SelectListItem> UrlTypes { get; set; }

        public IEnumerable<SelectListItem> Applications { get; set; }
    }
}
