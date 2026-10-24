using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models
{
    public class AddTicketProviderViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }
    }
}
