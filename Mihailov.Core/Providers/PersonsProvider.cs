using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
{
    public class PersonsProvider : IPersonsProvider
    {
        private readonly IPersonsRepository repository;

        public PersonsProvider(IPersonsRepository personsRepository)
        {
            this.repository = personsRepository;
        }

        public IEnumerable<Person> GetAll()
        {
            IEnumerable<Person> persons = this.repository.GetAll()
                                                         .ToList();
            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.repository.GetById(id);
            return person;
        }

        public Person GetByName(string name)
        {
            Person person = this.repository.GetByName(name.Trim());
            return person;
        }
    }
}
