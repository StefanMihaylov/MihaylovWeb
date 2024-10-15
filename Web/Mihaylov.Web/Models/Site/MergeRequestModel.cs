using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class MergeRequestModel
    {
        [Required]
        public long? From { get; set; }

        [Required]
        public long? To { get; set; }
    }
}
