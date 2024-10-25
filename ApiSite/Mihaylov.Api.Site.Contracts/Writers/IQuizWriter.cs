using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Writers
{
    public interface IQuizWriter
    {
        Task<QuizQuestion> AddOrUpdateQuestionAsync(QuizQuestion input);

        Task<QuizPhrase> AddOrUpdatePhraseAsync(QuizPhrase input);

        Task<QuizAnswer> AddOrUpdateAnswersAsync(QuizAnswer input);

        Task RemoveQuizAnswerAsync(long id);
    }
}
