using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddQuizPhraseModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(QuizPhrase.TextMaxLength)]
        public string Text { get; set; }

        public int? OrderId { get; set; }
    }
}
