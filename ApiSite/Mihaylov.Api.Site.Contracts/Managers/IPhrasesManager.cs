using System.Collections.Generic;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IPhrasesManager
    {
        IEnumerable<QuizPhrase> GetAllPhrases();
    }
}
