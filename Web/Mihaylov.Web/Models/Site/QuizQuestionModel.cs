using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class QuizQuestionModel
    {
        public int? Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
