using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class AnswersExtended
    {
        public long PersonId { get; }

        public IEnumerable<QuizAnswer> Answers { get; }

        public AnswersExtended(long personId, IEnumerable<QuizAnswer> answers)
        {
            PersonId = personId;
            Answers = answers;
        }
    }
}
