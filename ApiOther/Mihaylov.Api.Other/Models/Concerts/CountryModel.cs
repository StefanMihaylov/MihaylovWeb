using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Base;

namespace Mihaylov.Api.Other.Models.Concerts
{
    public class CountryModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.CountryNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }

        [MaxLength(3)]
        public string Code { get; set; }
    }
}
