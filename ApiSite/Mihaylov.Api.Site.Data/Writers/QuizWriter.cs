using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Managers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class QuizWriter : BaseWriter, IQuizWriter
    {
        private readonly IQuizRepository _repository;

        public QuizWriter(ILoggerFactory loggerFactory, IMemoryCache memoryCache, IQuizRepository phrasesRepository)
            : base(loggerFactory, memoryCache)
        {
            _repository = phrasesRepository;
        }

        public async Task<QuizQuestion> AddOrUpdateQuestionAsync(QuizQuestion input)
        {
            QuizQuestion question = await _repository.AddQuizQuestionAsync(input).ConfigureAwait(false);

            ClearCache<QuizManager, QuizQuestion>();

            return question;
        }

        public async Task<QuizPhrase> AddOrUpdatePhraseAsync(QuizPhrase input)
        {
            if (!input.OrderId.HasValue)
            {
                int maxOrderId = await _repository.GetMaxOrderIdAsync().ConfigureAwait(false);

                input.OrderId = maxOrderId + 1;
            }

            QuizPhrase phrase = await _repository.AddQuizPhraseAsync(input).ConfigureAwait(false);

            ClearCache<QuizManager, QuizPhrase>();

            return phrase;
        }

        public Task<QuizAnswer> AddOrUpdateAnswersAsync(QuizAnswer input)
        {
            var answer = _repository.AddQuizAnswerAsync(input);

            return answer;
        }

        public Task RemoveQuizAnswerAsync(long id)
        {
            return _repository.RemoveQuizAnswerAsync(id);
        }
    }
}
