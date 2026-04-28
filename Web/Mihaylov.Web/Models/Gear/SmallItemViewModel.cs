using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Gear;

public class SmallItemViewModel
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; }
}
