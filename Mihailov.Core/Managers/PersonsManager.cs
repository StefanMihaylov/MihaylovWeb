using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsProvider provider;

        private readonly ConcurrentDictionary<int, Person> personsById;
        private readonly ConcurrentDictionary<string, Person> personsByName;

        public PersonsManager(IPersonsProvider personsProvider)
        {
            this.provider = personsProvider;
            this.personsById = new ConcurrentDictionary<int, Person>();
            this.personsByName = new ConcurrentDictionary<string, Person>(StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<Person> GetAllPersons()
        {
            IEnumerable<Person> persons = this.personsByName.Values;
            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.personsById.GetOrAdd(id, (newId) =>
            {
                Person newPerson = this.provider.GetById(newId);
                return newPerson;
            });

            return person;
        }

        public Person GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            string key = name.Trim();
            Person person = this.personsByName.GetOrAdd(key, (newName) =>
            {
                try
                {
                    Person newPerson = this.provider.GetByName(newName);
                    return newPerson;
                }
                catch (Exception)
                {
                    return null;
                }
            });

            if (person == null)
            {
                this.personsByName.TryRemove(key, out person);
            }

            return person;
        }

        public PersonStatistics GetStatictics()
        {
            PersonStatistics statistics = this.provider.GetStatictics();
            return statistics;
        }
    }
}
