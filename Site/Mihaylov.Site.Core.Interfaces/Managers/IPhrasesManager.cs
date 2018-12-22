using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPhrasesManager
    {
        IEnumerable<Phrase> GetAllPhrases();
    }
}