using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Concerts;

public class AddBandViewModel
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int? CountryId { get; set; }

    public int Page { get; set; } = 1;
}
