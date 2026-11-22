using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear;

public class SmallItemViewModel
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; }
}
