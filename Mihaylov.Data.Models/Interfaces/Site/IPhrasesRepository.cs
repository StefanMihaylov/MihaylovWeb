using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface IPhrasesRepository
    {
        Phrase AddOrUpdatePhrase(Phrase inputPhrase, out bool isNew);

        IEnumerable<Phrase> GetAll();

        Phrase GetById(int id);
    }
}