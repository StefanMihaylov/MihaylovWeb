using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddQuizQuestionModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(QuizQuestion.ValueMaxLength)]
        public string Value { get; set; }
    }
}
