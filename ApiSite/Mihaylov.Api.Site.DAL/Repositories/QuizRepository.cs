using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Database;
using DB = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.DAL.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ILogger _logger;
        private readonly SiteDbContext _context;

        public QuizRepository(ILoggerFactory loggerFactory, SiteDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<IEnumerable<QuizQuestion>> GetAllQuestionsAsync()
        {
            var query = _context.QuizQuestions.AsNoTracking()
                                              .OrderBy(q => q.QuestionId)
                                              .AsQueryable();

            var questions = await query.ProjectToType<QuizQuestion>()
                                     .ToListAsync()
                                     .ConfigureAwait(false);

            return questions;
        }

        public async Task<QuizQuestion> GetQuestionAsync(int questionId)
        {
            var query = _context.QuizQuestions.AsNoTracking()
                                        .Where(q => q.QuestionId == questionId)
                                        .AsQueryable();

            var question = await query.ProjectToType<QuizQuestion>()
                                     .FirstOrDefaultAsync()
                                     .ConfigureAwait(false);
            return question;
        }

        public async Task<QuizQuestion> AddQuizQuestionAsync(QuizQuestion input)
        {
            input.Value = input.Value?.Trim();

            try
            {
                var dbModel = await _context.QuizQuestions
                                    .Where(t => t.QuestionId == input.Id)
                                    .FirstOrDefaultAsync()
                                    .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.QuizQuestion();
                    _context.QuizQuestions.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<QuizQuestion>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update QuizQuestion. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<QuizPhrase>> GetAllPhrasesAsync()
        {
            var query = _context.QuizPhrases.AsNoTracking()
                                            .OrderBy(p => p.OrderId)
                                            .AsQueryable();

            var phrases = await query.ProjectToType<QuizPhrase>()
                                     .ToListAsync()
                                     .ConfigureAwait(false);

            return phrases;
        }

        public async Task<QuizPhrase> AddQuizPhraseAsync(QuizPhrase input)
        {
            input.Text = input.Text?.Trim();

            try
            {
                var dbModel = await _context.QuizPhrases
                                .Where(t => t.PhraseId == input.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.QuizPhrase();
                    _context.QuizPhrases.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<QuizPhrase>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update QuizPhrase. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<int> GetMaxOrderIdAsync()
        {
            int maxOrderId = await _context.QuizPhrases
                                                .Select(p => p.OrderId)
                                                .DefaultIfEmpty()
                                                .MaxAsync()
                                                .ConfigureAwait(false);

            return maxOrderId;
        }

        public async Task<IEnumerable<HalfType>> GetAllHalfTypesAsync()
        {
            var query = _context.HalfTypes.AsNoTracking()
                                          .OrderBy(p => p.HalfTypeId)
                                          .AsQueryable();

            var halfTypes = await query.ProjectToType<HalfType>()
                                       .ToListAsync()
                                       .ConfigureAwait(false);

            return halfTypes;
        }

        public async Task<IEnumerable<UnitShort>> GetAllUnitsAsync()
        {
            var query = _context.Units.AsNoTracking()
                                      .OrderBy(p => p.UnitId)
                                      .AsQueryable();

            var units = await query.ProjectToType<UnitShort>()

                                   .ToListAsync()
                                   .ConfigureAwait(false);

            return units;
        }

        public async Task<IEnumerable<QuizAnswer>> GetQuizAnswersAsync(long personId)
        {
            var query = GetQuizAnswerQuery()
                              .Where(a => a.PersonId == personId)
                              .OrderBy(a => a.AskDate)
                                .ThenBy(a => a.QuestionId);

            var answers = await query.ProjectToType<QuizAnswer>()
                                   .ToListAsync()
                                   .ConfigureAwait(false);

            return answers;
        }

        public async Task<QuizAnswer> GetQuizAnswerAsync(long id)
        {
            IQueryable<DB.QuizAnswer> query = GetQuizAnswerQuery()
                                                .Where(a => a.QuizAnswerId == id);

            var answer = await query.ProjectToType<QuizAnswer>()
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);

            return answer;
        }

        public async Task<QuizAnswer> AddQuizAnswerAsync(QuizAnswer input)
        {
            input.Details = input.Details?.Trim();

            try
            {
                var dbModel = await _context.QuizAnswers
                                        .Where(t => t.QuizAnswerId == input.Id)
                                        .FirstOrDefaultAsync()
                                        .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.QuizAnswer();
                    _context.QuizAnswers.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return await GetQuizAnswerAsync(dbModel.QuizAnswerId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update QuizAnswer. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemoveQuizAnswerAsync(long id)
        {
            var answer = await GetQuizAnswerQuery()
                                .Where(a => a.QuizAnswerId == id)
                                .Where(a => a.QuestionId == 1)
                                .Where(a => a.Details == null || a.Details.Length == 0)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

            if (answer != null)
            {
                _context.QuizAnswers.Remove(answer);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private IQueryable<DB.QuizAnswer> GetQuizAnswerQuery()
        {
            var query = _context.QuizAnswers.AsNoTracking()
                                            .Include(c => c.Question)
                                            .Include(c => c.HalfType)
                                            .Include(c => c.Unit)
                                                .ThenInclude(u => u.BaseUnit);
            return query;
        }
    }
}
