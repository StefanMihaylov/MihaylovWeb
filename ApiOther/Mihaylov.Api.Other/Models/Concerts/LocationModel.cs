using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Base;

namespace Mihaylov.Api.Other.Models.Concerts
{
    public class LocationModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.LocationNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
