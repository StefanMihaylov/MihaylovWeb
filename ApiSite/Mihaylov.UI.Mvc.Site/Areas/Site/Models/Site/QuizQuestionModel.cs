using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class QuizQuestionModel
    {
        public int? Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
