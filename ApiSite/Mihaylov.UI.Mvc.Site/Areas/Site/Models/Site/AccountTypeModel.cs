using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class AccountTypeModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
