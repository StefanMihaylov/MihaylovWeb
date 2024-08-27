using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Web.Models.Cluster
{
    public class AddAdditionalModel
    {
        [Required]
        public int? ApplicationId { get; set; }

        public string File { get; set; }

        public string Pod { get; set; }


        public IEnumerable<SelectListItem> Applications { get; set; }
    }
}
