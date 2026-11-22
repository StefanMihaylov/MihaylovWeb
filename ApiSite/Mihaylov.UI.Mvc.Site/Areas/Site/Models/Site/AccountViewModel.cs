using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class AccountViewModel
    {
        public long? Id { get; set; }

        [Required]
        public long? PersonId { get; set; }
    }
}
