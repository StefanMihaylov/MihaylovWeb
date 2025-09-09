using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class DefaultFilterModel
    {
        public int? Id { get; set; }

        [Required]
        public bool? IsEnabled { get; set; }

        public int? AccountTypeId { get; set; }

        public int? StatusId { get; set; }

        [Required]
        public bool? IsArchive { get; set; }

        public bool? IsPreview { get; set; }
    }
}
