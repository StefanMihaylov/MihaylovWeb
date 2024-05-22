using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Concerts
{
    public class AddTicketProviderViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
