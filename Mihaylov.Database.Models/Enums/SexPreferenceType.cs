using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Database.Models.Enums
{
    public enum SexPreferenceType
    {
        [Display(Name = "Unknown")]
        Unknown = 0,

        [Display(Name = "Straight")]
        Straight = 1,

        Gay,

        Bisexual,

        Bicurious,


    }
}
