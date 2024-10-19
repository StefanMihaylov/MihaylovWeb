using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class QuizPhraseModel
    {
        public int? Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
