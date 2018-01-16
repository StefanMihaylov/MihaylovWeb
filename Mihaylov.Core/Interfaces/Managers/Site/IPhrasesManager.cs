using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPhrasesManager
    {
        IEnumerable<Phrase> GetAllPhrases();
    }
}