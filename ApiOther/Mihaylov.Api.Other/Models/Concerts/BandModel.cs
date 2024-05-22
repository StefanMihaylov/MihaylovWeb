using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Show;

namespace Mihaylov.Api.Other.Models.Concerts
{
    public class BandModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.BandNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
