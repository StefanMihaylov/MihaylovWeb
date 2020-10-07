using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPhrasesWriter
    {
        Task<Phrase> AddOrUpdateAsync(Phrase inputPhrase);
    }
}