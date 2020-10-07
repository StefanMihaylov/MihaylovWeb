using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface IPhrasesRepository
    {
        Task<Phrase> AddOrUpdatePhraseAsync(Phrase inputPhrase);//, out bool isNew);

        Task<IEnumerable<Phrase>> GetAllAsync();

        Task<Phrase> GetByIdAsync(int id);

        Task<int> GetMaxOrderId();
    }
}