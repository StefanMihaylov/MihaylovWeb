using System;

namespace Mihaylov.Api.Site.Database.Models
{
    public class QuizAnswer
    {
        public long QuizAnswerId { get; set; }

        public long PersonId { get; set; }

        public Person Person { get; set; }

        public DateTime AskDate { get; set; }

        public int QuestionId { get; set; }

        public QuizQuestion Question { get; set; }

        public decimal? Value { get; set; }

        public int? UnitId { get; set; }

        public Unit Unit { get; set; }

        public int? HalfTypeId { get; set; }

        public HalfType HalfType { get; set; }

        public string Details { get; set; }
    }
}
