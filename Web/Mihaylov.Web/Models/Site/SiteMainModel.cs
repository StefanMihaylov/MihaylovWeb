using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class SiteMainModel
    {
        public PersonGrid Grid { get; set; }

        public PersonStatistics Statistics { get; set; }

        public IEnumerable<QuizPhrase> QuizPhrases { get; set; }
    }
}
