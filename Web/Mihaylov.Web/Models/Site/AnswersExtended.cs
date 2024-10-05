using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class AnswersExtended
    {
        public long PersonId { get; set; }

        public IEnumerable<QuizAnswer> Answers { get; set; }
    }
}
