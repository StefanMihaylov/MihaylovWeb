using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Database.Models.Enums
{
    public enum UnitType
    {
        [Display(Name = "cm")]
        CM = 1,

        [Display(Name = "inch")]
        Inch,
    }
}
