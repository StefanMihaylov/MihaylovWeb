using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsProvider provider;
        private readonly ILogger logger;

        private readonly ConcurrentDictionary<int, Person> personsById;
        private readonly ConcurrentDictionary<string, Person> personsByName;

        public PersonsManager(IPersonsProvider personsProvider, ILogger logger)
        {
            this.provider = personsProvider;
            this.logger = logger;

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

            try
            {
                string key = name.Trim();
                Person person = this.personsByName.GetOrAdd(key, (newName) =>
                {
                    Person newPerson = this.provider.GetByName(newName);
                    return newPerson;
                });

                return person;
            }
            catch (ApplicationException)
            {
                this.logger.Error($"Person with name '{name}' not found.");
                return null;
            }
            catch (Exception ex)
            {
                this.logger.ErrorException($"Error in getting the person by name '{name}'", ex);
                return null;
            }
        }

        public PersonStatistics GetStatictics()
        {
            PersonStatistics statistics = this.provider.GetStatictics();
            return statistics;
        }
    }
}
