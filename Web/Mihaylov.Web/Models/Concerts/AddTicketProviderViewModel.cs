using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Concerts
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
