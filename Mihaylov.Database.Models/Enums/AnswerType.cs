using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Database.Models.Enums
{
    public enum AnswerType
    {
        [Display(Name = "Not Answered")]
        NotAnswered = 1,

        [Display(Name = "Answered")]
        Answered,

        [Display(Name = "I Don't Know")]
        IDontKnow,

        [Display(Name = "Banned")]
        Banned,

        [Display(Name = "Kicked")]
        Kicked,
    }
}
