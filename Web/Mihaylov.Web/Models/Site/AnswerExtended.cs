using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class AnswerExtended : QuizAnswer
    {
        public AnswerExtended(QuizAnswer answer)
        {
            Id = answer.Id;
            PersonId = answer.PersonId;
            AskDate = answer.AskDate;
            QuestionId = answer.QuestionId;
            Question = answer.Question;
            Value = answer.Value;
            UnitId = answer.UnitId;
            Unit = answer.Unit;
            ConvertedValue = answer.ConvertedValue;
            ConvertedUnit = answer.ConvertedUnit;
            HalfTypeId = answer.HalfTypeId;
            HalfType = answer.HalfType;
            Details = answer.Details;
        }        

        public IEnumerable<QuizQuestion> Questions { get; set; }

        public IEnumerable<UnitShort> Units { get; set; }

        public IEnumerable<HalfType> HalfTypes { get; set; }
    }
}
