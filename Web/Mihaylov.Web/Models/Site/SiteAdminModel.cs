using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class SiteAdminModel
    {
        public IEnumerable<QuizQuestion> Questions { get; set; }

        public IEnumerable<AccountType> AccountTypes { get; set; }

        public IEnumerable<QuizPhrase> QuizPhrases { get; set; }

        public IEnumerable<DefaultFilter> DefaultFilters { get; set; }
    }
}
