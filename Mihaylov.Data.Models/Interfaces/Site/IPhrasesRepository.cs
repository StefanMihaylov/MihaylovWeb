using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface IPhrasesRepository
    {
        Phrase AddOrUpdatePhrase(Phrase inputPhrase);

        IEnumerable<Phrase> GetAll();

        Phrase GetById(int id);
    }
}