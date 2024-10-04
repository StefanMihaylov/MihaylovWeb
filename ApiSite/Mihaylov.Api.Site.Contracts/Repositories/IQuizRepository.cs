using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface IQuizRepository
    {
        Task<IEnumerable<QuizQuestion>> GetAllQuestionsAsync();

        Task<QuizQuestion> AddQuizQuestionAsync(QuizQuestion input);

        Task<IEnumerable<QuizPhrase>> GetAllPhrasesAsync();

        Task<QuizPhrase> AddQuizPhraseAsync(QuizPhrase input);

        Task<int> GetMaxOrderIdAsync();

        Task<IEnumerable<HalfType>> GetAllHalfTypesAsync();

        Task<IEnumerable<UnitShort>> GetAllUnitsAsync();

        Task<IEnumerable<QuizAnswer>> GetQuizAnswersAsync(long personId);

        Task<QuizAnswer> GetQuizAnswerAsync(long id);

        Task<QuizAnswer> AddQuizAnswerAsync(QuizAnswer input);
    }
}
