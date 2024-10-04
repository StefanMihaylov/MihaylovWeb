using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddAccountTypeModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(AccountType.NameMaxLength)]
        public string Name { get; set; }
    }
}
