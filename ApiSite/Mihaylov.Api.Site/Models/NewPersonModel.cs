using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Site.Models
{
    public class NewPersonModel
    {
        public int? AccountTypeId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public bool? IsPreview { get; set; }
    }
}
