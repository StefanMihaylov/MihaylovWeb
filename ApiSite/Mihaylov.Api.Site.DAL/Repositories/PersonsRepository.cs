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
            Person person = await this._context.Persons
                                               .Where(p => p.PersonId == id)
                                               .To<Person>()
                                               .FirstOrDefaultAsync()
                                               .ConfigureAwait(false);

            return person;
        }

        public async Task<Person> GetByAccoutUserNameAsync(string username)
        {
            Person person = await this._context.Accounts
                                               .Where(a => a.Username == username)
                                               .Select(a => a.Person)
                                               .To<Person>()
                                               .FirstOrDefaultAsync()
                                               .ConfigureAwait(false);

            return person;
        }

        public async Task<Person> AddOrUpdatePersonAsync(Person inputPerson)
        {
            DB.Person person;
            if (inputPerson.Id == 0)
            {
                person = new DB.Person();
                this._context.Persons.Add(person);
            }
            else
            {
                person = await this._context.Persons.Where(p => p.PersonId == inputPerson.Id)
                                                    .FirstOrDefaultAsync()
                                                    .ConfigureAwait(false);
            }

            // inputPerson.Update(person);

            // await this._context.SaveChangesAsync().ConfigureAwait(false);

            Person personDTO = await this.GetByIdAsync(person.PersonId);
            return personDTO;
        }

        public async Task<PersonStatistics> GetStaticticsAsync()
        {
            //IQueryable<DAL.Person> persons = this.All().Where(p => p.AnswerType.IsAsked);

            //var countDictionary = persons.GroupBy(p => new { Description = p.AnswerType.Description, Id = p.AnswerTypeId })
            //                             .Select(g => new { Key = g.Key.Description, Value = g.Count() })
            //                             .OrderBy(g => g.Key)
            //                             .ToDictionary(r => r.Key, r => r.Value);

            //IQueryable<decimal> answers = persons.Where(p => p.AnswerConverted.HasValue)
            //                                     .Select(p => p.AnswerConverted.Value);

            //var statistics = new PersonStatistics()
            //{
            //    CountDictionary = countDictionary,
            //    Average = answers.Average(),
            //    Min = answers.Min(),
            //    Max = answers.Max(),
            //    TotalCount = this.All().Count(),
            //    Disabled = persons.Count(p => p.IsAccountDisabled),
            //};

            //return statistics;

            return new PersonStatistics();
        }
    }
}
