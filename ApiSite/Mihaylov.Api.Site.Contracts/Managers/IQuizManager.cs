using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IQuizManager
    {
        Task<IEnumerable<QuizQuestion>> GetAllQuestionsAsync();

        Task<IEnumerable<QuizPhrase>> GetAllPhrasesAsync();

        Task<IEnumerable<HalfType>> GetAllHalfTypesAsync();

        Task<IEnumerable<UnitShort>> GetAllUnitsAsync();

        Task<IEnumerable<QuizAnswer>> GetQuizAnswersAsync(long personId);
    }
}
