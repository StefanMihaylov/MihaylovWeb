using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class PersonsRepository : GenericRepository<DAL.Person, IMihaylovDbContext>, IPersonsRepository
    {
        public PersonsRepository(IMihaylovDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Person> GetAll()
        {
            IEnumerable<Person> persons = this.All()
                                              .Select(Person.FromDb)
                                              .AsQueryable();

            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.All()
                                .Where(p => p.PersonId == id)
                                .Select(Person.FromDb)
                                .FirstOrDefault();

            return person;
        }

        public Person GetByName(string username)
        {
            Person person = this.All()
                                .Where(p => p.Username == username)
                                .Select(Person.FromDb)
                                .FirstOrDefault();

            return person;
        }

        public Person AddPerson(Person inputPerson)
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
