using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class QuizPhraseModel
    {
        public int? Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
