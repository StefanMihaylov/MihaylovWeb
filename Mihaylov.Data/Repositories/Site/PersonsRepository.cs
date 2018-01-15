using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Common.Mapping;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
{
    public class PersonsRepository : GenericRepository<DAL.Person, ISiteDbContext>, IPersonsRepository
    {
        public PersonsRepository(ISiteDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Person> GetAll()
        {
            IEnumerable<Person> persons = this.All()
                                              .To<Person>()
                                              .AsQueryable();

            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.All()
                                .Where(p => p.PersonId == id)
                                .To<Person>()
                                .FirstOrDefault();

            return person;
        }

        public Person GetByName(string username)
        {
            Person person = this.All()
                                .Where(p => p.Username == username)
                                .To<Person>()
                                .FirstOrDefault();

            return person;
        }

        public Person AddOrUpdatePerson(Person inputPerson)
        {
            DAL.Person person;
            if (inputPerson.Id == 0)
            {
                person = new DAL.Person();
                this.Add(person);
            }
            else
            {
                person = this.GetById((object)inputPerson.Id);
            }

            inputPerson.Update(person);

            this.Context.SaveChanges();

            Person personDTO = this.GetById(person.PersonId);
            return personDTO;
        }

        public PersonStatistics GetStatictics()
        {
            IQueryable<DAL.Person> persons = this.All().Where(p => p.AnswerType.IsAsked);

            var countDictionary = persons.GroupBy(p => new { Description = p.AnswerType.Description, Id = p.AnswerTypeId })
                                         .Select(g => new { Key = g.Key.Description, Value = g.Count() })
                                         .OrderBy(g => g.Key)
                                         .ToDictionary(r => r.Key, r => r.Value);

            IQueryable<decimal> answers = persons.Where(p => p.AnswerConverted.HasValue)
                                                 .Select(p => p.AnswerConverted.Value);

            var statistics = new PersonStatistics()
            {
                CountDictionary = countDictionary,
                Average = answers.Average(),
                Min = answers.Min(),
                Max = answers.Max(),
                TotalCount = this.All().Count(),
                Disabled = persons.Count(p => p.IsAccountDisabled),
            };

            return statistics;
        }
    }
}
