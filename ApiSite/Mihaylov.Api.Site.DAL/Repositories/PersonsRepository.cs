using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Mapping;
using DB = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.DAL.Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ILogger _logger;
        private readonly SiteDbContext _context;

        public PersonsRepository(ILoggerFactory loggerFactory, SiteDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<Grid<Person>> GetAllPersonsAsync(GridRequest request)
        {
            try
            {
                var query = GetPersonBaseQuery()
                                        .Select(p => new
                                        {
                                            Person = p,
                                            FullName = p.Details != null ? $"{p.Details.FirstName} {p.Details.MiddleName} {p.Details.LastName}" : null,
                                        })
                                        .OrderByDescending(c => c.Person.CreatedOn)
                                        .AsQueryable();

                if (request.AccountTypeId.HasValue)
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.AccountTypeId == request.AccountTypeId));
                }

                if (request.AccountStatusId.HasValue)
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.StatusId == request.AccountStatusId));
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.FullName.Contains(request.Name));
                }

                if (!string.IsNullOrWhiteSpace(request.AccountName))
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.Username.Contains(request.Name)));
                }


                var count = await query.CountAsync().ConfigureAwait(false);

                if (request.Page.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.Page.Value - 1) * request.PageSize.Value)
                                 .Take(request.PageSize.Value)
                                 .AsQueryable();
                }

                var persons = await query.Select(p => p.Person)
                                         .ProjectToType<Person>()
                                         .ToListAsync()
                                         .ConfigureAwait(false);

                var result = new Grid<Person>()
                {
                    Data = persons,
                    Pager = new Pager(request, count),
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Persons. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetAllForUpdateAsync()
        {
            var persons = await this._context.Persons
                                             //.Where(p => p.IsAccountDisabled == false)
                                             .Where(p => p.ModifiedOn < DateTime.UtcNow.AddDays(-1))
                                             .To<Person>()
                                             .ToListAsync()
                                             .ConfigureAwait(false);

            return persons;
        }

        public async Task<Person> GetByIdAsync(long id)
        {
            var query = GetPersonBaseQuery().Where(p => p.PersonId == id).AsQueryable();

            Person person = await query.ProjectToType<Person>()
                                       .FirstOrDefaultAsync()
                                       .ConfigureAwait(false);

            return person;
        }

        public Task<Person> GetByAccoutUserNameAsync(string username)
        {
            //Person person = await this._context.Accounts
            //                                   .Where(a => a.Username == username)
            //                                   .Select(a => a.Person)
            //                                   .To<Person>()
            //                                   .FirstOrDefaultAsync()
            //                                   .ConfigureAwait(false);

            //return person;

            throw new NotImplementedException();
        }

        public async Task<Person> AddOrUpdatePersonAsync(Person input)
        {
            if (input.Detais != null)
            {
                input.Detais.FirstName = input.Detais.FirstName?.Trim();
                input.Detais.MiddleName = input.Detais.MiddleName?.Trim();
                input.Detais.LastName = input.Detais.LastName?.Trim();
                input.Detais.OtherNames = input.Detais.OtherNames?.Trim();
            }

            input.Comments = input.Comments?.Trim();

            try
            {
                var dbModel = await _context.Persons
                                .Where(p => p.PersonId == input.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Person();
                    _context.Persons.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

               // await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<Person>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Person. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PersonStatistics> GetStaticticsAsync()
        {
            var mainQuestionId = 2;
            var activeStates = new List<int> { 1, 2 };

            var totalPersonCount = await _context.Persons.Include(p => p.Accounts)
                                               .CountAsync()
                                               .ConfigureAwait(false);

            var queryAnswers = _context.QuizAnswers.Include(a => a.Unit)
                                            .Where(a => a.QuestionId == mainQuestionId &&
                                                        a.UnitId.HasValue &&
                                                        a.Value.HasValue)
                                            .Select(a => a.Value.Value * (a.Unit.ConversionRate ?? 1))
                                            .AsQueryable();

            var average = await queryAnswers.AverageAsync().ConfigureAwait(false);
            var min = await queryAnswers.MinAsync().ConfigureAwait(false);
            var max = await queryAnswers.MaxAsync().ConfigureAwait(false);

            var answers = await _context.QuizAnswers.Include(a => a.Unit)
                                                    .Where(a => a.QuestionId == mainQuestionId)
                                                    .GroupBy(a => a.UnitId.HasValue)
                                                    .Select(g => new PersonStatisticsPair<bool> { Key = g.Key, Value = g.Count() })
                                                    .OrderByDescending(a => a.Key)
                                                    .ToListAsync()
                                                    .ConfigureAwait(false);

            var accountTypes = await _context.Accounts.Include(a => a.AccountType)
                                                 .Where(a => a.PersonId.HasValue)
                                                 .GroupBy(a => a.AccountType.Name)
                                                 .Select(g => new PersonStatisticsPair<string> { Key = g.Key, Value = g.Count() })
                                                 .OrderBy(a => a.Key)
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            var accountStates = await _context.Accounts.Include(a => a.Status)
                                     .Where(a => a.PersonId.HasValue && a.StatusId.HasValue)
                                     .GroupBy(a => new { a.Status.Name, a.StatusId })
                                     .OrderBy(a => a.Key.StatusId)
                                     .Select(g => new PersonStatisticsPair<string> { Key = g.Key.Name, Value = g.Count() })
                                     .ToListAsync()
                                     .ConfigureAwait(false);

            var activeAccounts = await _context.Accounts.Include(a => a.Status)
                                     .Where(a => a.PersonId.HasValue && a.StatusId.HasValue)
                                     .Where(a => activeStates.Contains(a.StatusId.Value))
                                     .CountAsync()
                                     .ConfigureAwait(false);

            var statistics = new PersonStatistics()
            {
                Answers = answers,
                AccountTypes = accountTypes,
                States = accountStates,
                Average = average,
                Min = min,
                Max = max,
                TotalPersonCount = totalPersonCount,
                ActiveAccountCount = activeAccounts,
            };

            return statistics;
        }

        private IQueryable<DB.Person> GetPersonBaseQuery()
        {
            var query = _context.Persons.AsNoTracking()
                                        .Include(c => c.Details)
                                        .Include(c => c.Location)
                                            .ThenInclude(s => s.CountryState)
                                        .Include(c => c.Ethnicity)
                                        .Include(c => c.Orientation)
                                        .Include(c => c.Country)
                                        .Include(c => c.Accounts)
                                            .ThenInclude(a => a.AccountType)
                                        .Include(c => c.Accounts)
                                            .ThenInclude(a => a.Status)
                                        .AsQueryable();

            return query;
        }
    }
}
