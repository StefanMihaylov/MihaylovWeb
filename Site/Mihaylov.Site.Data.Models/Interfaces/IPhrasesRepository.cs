using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface IPhrasesRepository
    {
        Phrase AddOrUpdatePhrase(Phrase inputPhrase, out bool isNew);

        IEnumerable<Phrase> GetAll();

        Phrase GetById(int id);
    }
}