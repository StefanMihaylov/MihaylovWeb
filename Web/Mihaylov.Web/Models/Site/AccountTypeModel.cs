using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class AccountTypeModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
