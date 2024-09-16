using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.Data.Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly SiteDbContext _context;

        public PersonsRepository(SiteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var persons = await this._context.Persons
                                             .To<Person>()
                                             .ToListAsync()
                                             .ConfigureAwait(false);
            return persons;
        }

        public async Task<IEnumerable<Person>> Search(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            var query = this._context.Persons.AsQueryable();

            //if (descOrder)
            //{
            //    query = query.OrderByDescending(p => p.AskDate);
            //}
            //else
            //{
            //    query = query.OrderBy(p => p.AskDate);
            //}

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skipCount = pageSize.Value * pageNumber.Value;
                query = query.Skip(skipCount).Take(pageSize.Value);
            }

            IEnumerable<Person> persons = await query.To<Person>()
                                                     .ToListAsync()
                                                     .ConfigureAwait(false);
            return persons;
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
            DAL.Person person;
            if (inputPerson.Id == 0)
            {
                person = new DAL.Person();
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
