using System;
using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddQuizAnswerModel
    {
        public long? Id { get; set; }

        [Required]
        public long? PersonId { get; set; }

        [Required]
        public DateTime? AskDate { get; set; }

        [Required]
        public int? QuestionId { get; set; }

        public decimal? Value { get; set; }

        public int? UnitId { get; set; }

        public int? HalfTypeId { get; set; }

        [StringLength(QuizAnswer.DetailsMaxLength)]
        public string Details { get; set; }
    }
}
