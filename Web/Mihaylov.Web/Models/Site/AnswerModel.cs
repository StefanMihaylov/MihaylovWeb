using System;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class AnswerModel
    {
        [Required]
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

        public string Details { get; set; }
    }
}
