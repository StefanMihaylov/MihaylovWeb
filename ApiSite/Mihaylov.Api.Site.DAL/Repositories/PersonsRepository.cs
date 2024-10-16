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
                                            FullName = p.Details != null ? p.Details.FirstName + " " + p.Details.MiddleName + " " + p.Details.LastName : null,
                                        })
                                        .OrderByDescending(c => c.Person.CreatedOn)
                                        .AsQueryable();

                if (request.AccountTypeId.HasValue)
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.AccountTypeId == request.AccountTypeId));
                }

                if (request.StatusId.HasValue)
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.StatusId == request.StatusId));
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.FullName.Contains(request.Name));
                }

                if (!string.IsNullOrWhiteSpace(request.AccountName))
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.Username.Contains(request.AccountName)));
                }

                if (!string.IsNullOrWhiteSpace(request.AccountNameExact))
                {
                    query = query.Where(p => p.Person.Accounts.Any(a => a.Username == request.AccountNameExact));
                }

                // temp filters
                var hasAnyFilter = request.StatusId.HasValue || request.AccountTypeId.HasValue ||
                        !string.IsNullOrWhiteSpace(request.Name) || !string.IsNullOrWhiteSpace(request.AccountName) ||
                        !string.IsNullOrWhiteSpace(request.AccountNameExact);

                if (!hasAnyFilter)
                {
                    // query = query.Where(p => !string.IsNullOrEmpty(p.Person.Comments));
                    // query = query.Where(p => p.Person.PersonId == 5511 || p.Person.PersonId == 5486 || p.Person.PersonId == 5475);
                    // query = query.Where(p => !p.Person.Accounts.Any(a => a.StatusId == 3 && a.ModifiedOn > DateTime.Now.AddMonths(-1)));
                    // query = query.Where(p => p.Person.Accounts.Any(a => a.CreatedOn > new DateTime(2018, 1, 7)));
                    // query = query.Where(p => p.Person.Answers.Any(a => a.QuestionId == 1));
                    // query = query.Where(p => p.Person.Answers.Any(a => a.AskDate > new DateTime(2024, 1, 7)));
                }

                var count = await query.CountAsync().ConfigureAwait(false);

                (query, request.Page) = GetPage(query, request.PageSize, request.Page, count);

                var persons = await query.Select(p => p.Person)
                                         .ProjectToType<Person>()
                                         .ToListAsync()
                                         .ConfigureAwait(false);

                var result = new Grid<Person>()
                {
                    Request = request,
                    Data = persons,
                    Pager = new Pager(request.Page, request.PageSize, count),
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Persons. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Person> GetPersonAsync(long id)
        {
            var query = GetPersonBaseQuery().Where(p => p.PersonId == id).AsQueryable();

            Person person = await query.ProjectToType<Person>()
                                       .FirstOrDefaultAsync()
                                       .ConfigureAwait(false);

            return person;
        }

        public async Task<Person> AddOrUpdatePersonAsync(Person input)
        {
            input.Comments = input.Comments?.Trim();

            if (input.Details != null)
            {
                input.Details.FirstName = input.Details.FirstName?.Trim();
                input.Details.MiddleName = input.Details.MiddleName?.Trim();
                input.Details.LastName = input.Details.LastName?.Trim();
                input.Details.OtherNames = input.Details.OtherNames?.Trim();
            }

            if (input.Location != null)
            {
                input.Location.Region = input.Location.Region?.Trim();
                input.Location.City = input.Location.City?.Trim();
                input.Location.Details = input.Location.Details?.Trim();
            }

            try
            {
                var dbModel = await _context.Persons
                                .Include(p => p.Details)
                                .Include(p => p.Location)
                                .Where(p => p.PersonId == input.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Person();
                    _context.Persons.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                if (input.Details != null)
                {
                    if (dbModel.Details == null)
                    {
                        dbModel.Details = new DB.PersonDetail();
                    }

                    dbModel.Details = input.Details.Adapt(dbModel.Details);
                }

                if (input.Location != null)
                {
                    if (dbModel.Location == null)
                    {
                        dbModel.Location = new DB.PersonLocation();
                    }

                    dbModel.Location = input.Location.Adapt(dbModel.Location);
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return await GetPersonAsync(dbModel.PersonId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Person. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task DeletePersonAsync(long id)
        {
            var hasAnswers = await _context.QuizAnswers.Where(a => a.PersonId == id)
                                                       .AnyAsync()
                                                       .ConfigureAwait(false);

            var hasAccounts = await _context.Accounts.Where(a => a.PersonId == id)
                                                     .AnyAsync()
                                                     .ConfigureAwait(false);
            if (!hasAnswers && !hasAccounts)
            {
                var person = await _context.Persons.Include(p => p.Details)
                                                   .Include(p => p.Location)
                                                   .Where(p => p.PersonId == id)
                                                   .SingleOrDefaultAsync();
                if (person == null)
                {
                    return;
                }

                if (person.Details != null)
                {
                    _context.PersonDetails.Remove(person.Details);
                }

                if (person.Location != null)
                {
                    _context.PersonLocations.Remove(person.Location);
                }

                _context.Persons.Remove(person);

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<Account> GetAccountAsync(long id)
        {
            var query = _context.Accounts.AsNoTracking()
                                         .Include(a => a.AccountType)
                                         .Include(a => a.Status)
                                         .Where(a => a.AccountId == id)
                                         .AsQueryable();

            var account = await query.ProjectToType<Account>()
                                     .FirstOrDefaultAsync()
                                     .ConfigureAwait(false);
            return account;
        }

        public async Task<UpdateAccounts> GetAllAccountsForUpdateAsync(int? batchSize)
        {
            var query = _context.Accounts.AsNoTracking()
                                             .Include(a => a.AccountType)
                                             .Include(a => a.Status)
                                             .Where(a => a.StatusId != 3 && a.StatusId != 7 &&
                                                         a.AccountTypeId == 3 &&
                                                        (a.ModifiedOn == null ||
                                                         a.ModifiedOn < DateTime.UtcNow.AddDays(-7)))
                                             .OrderBy(a => a.AccountId)
                                             .AsQueryable();

            var count = await query.CountAsync().ConfigureAwait(false);

            if (batchSize.HasValue && batchSize > 0)
            {
                query = query.Take(batchSize.Value);
            }

            var accounts = await query.ProjectToType<Account>()
                                      .ToListAsync()
                                      .ConfigureAwait(false);

            return new UpdateAccounts()
            {
                Accounts = accounts,
                BatchSize = batchSize,
                TotalCount = count,
            };
        }

        public async Task<Account> AddOrUpdateAccountAsync(Account input)
        {
            input.Username = input.Username?.Trim();
            input.DisplayName = input.DisplayName?.Trim();
            input.Details = input.Details?.Trim();

            try
            {
                var dbModel = await _context.Accounts
                                            .Where(p => p.AccountId == input.Id)
                                            .FirstOrDefaultAsync()
                                            .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Account();
                    _context.Accounts.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return await GetAccountAsync(dbModel.AccountId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Account. Error: {Message}", ex.Message);
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

        private static (IQueryable<T>, int?) GetPage<T>(IQueryable<T> query, int? pageSize, int? page, int count)
        {
            if (!pageSize.HasValue)
            {
                return (query, page);
            }

            if (!page.HasValue || page <= 0)
            {
                page = 1;
            }

            var pageMax = Pager.GetMaxPage(pageSize, count);

            if (pageMax > 0 && page > pageMax)
            {
                page = pageMax;
            }

            query = query.Skip((page.Value - 1) * pageSize.Value)
                     .Take(pageSize.Value)
                     .AsQueryable();

            return (query, page);
        }
    }
}
