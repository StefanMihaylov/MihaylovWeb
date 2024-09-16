using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface IQuizRepository
    {
        Task<QuizPhrase> AddOrUpdatePhraseAsync(QuizPhrase inputPhrase);//, out bool isNew);

        Task<IEnumerable<QuizPhrase>> GetAllAsync();

        Task<int> GetMaxOrderIdAsync();
    }
}
