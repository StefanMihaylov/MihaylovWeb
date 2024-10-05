using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class QuizManager : BaseManager, IQuizManager
    {
        private readonly IQuizRepository _repository;

        public QuizManager(ILoggerFactory loggerFactory, IMemoryCache memoryCache, IQuizRepository quizRepository)
            : base(loggerFactory, memoryCache)
        {
            _repository = quizRepository;
        }

        public async Task<IEnumerable<QuizQuestion>> GetAllQuestionsAsync()
        {
            var questions = await GetDataListAsync(_repository.GetAllQuestionsAsync).ConfigureAwait(false);
            return questions;
        }

        public Task<QuizQuestion> GetQuestionAsync(int questionId)
        {
            return _repository.GetQuestionAsync(questionId);
        }

        public async Task<IEnumerable<QuizPhrase>> GetAllPhrasesAsync()
        {
            var phrases = await GetDataListAsync(_repository.GetAllPhrasesAsync).ConfigureAwait(false);
            return phrases;
        }

        public async Task<IEnumerable<HalfType>> GetAllHalfTypesAsync()
        {
            var halfTypes = await GetDataListAsync(_repository.GetAllHalfTypesAsync).ConfigureAwait(false);
            return halfTypes;
        }

        public async Task<IEnumerable<UnitShort>> GetAllUnitsAsync()
        {
            var units = await GetDataListAsync(_repository.GetAllUnitsAsync).ConfigureAwait(false);
            return units;
        }

        public Task<IEnumerable<QuizAnswer>> GetQuizAnswersAsync(long personId)
        {
            return _repository.GetQuizAnswersAsync(personId);
        }

        public Task<QuizAnswer> GetQuizAnswerAsync(long id)
        {
            return _repository.GetQuizAnswerAsync(id);
        }
    }
}
