using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Database.Models.Enums
{
    public enum EthnicityType
    {
        [Display(Name = "Arab")]
        Arab = 1,

        Asian,

        Black,

        Indian,

        Mixed,

        White,

        Hispanic,
    }
}
