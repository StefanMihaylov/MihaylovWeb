using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Writers
{
    public interface IPhrasesWriter
    {
        Task<QuizPhrase> AddOrUpdateAsync(QuizPhrase inputPhrase);
    }
}
