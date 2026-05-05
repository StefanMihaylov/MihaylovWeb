using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Base;

namespace Mihaylov.Api.Other.Models.Concerts;

public class ConcertTypeModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(ModelConstants.ConcertTypeMaxLength, MinimumLength = 1)]
    public string Name { get; set; }
}
