using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Managers.Site
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

        public IEnumerable<Person> GetAllPersons(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            IEnumerable<Person> persons = this.personsByName.Values;

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                persons = this.FilterPage(persons, descOrder, pageNumber, pageSize);

                if (persons.Count() < pageSize.Value)
                {
                    IEnumerable<Person> dbPersons = this.provider.GetAll(descOrder, pageNumber, pageSize);
                    foreach (var dbPerson in dbPersons)
                    {
                        this.personsByName.TryAdd(dbPerson.Username, dbPerson);
                    }

                    persons = this.FilterPage(this.personsByName.Values, descOrder, pageNumber, pageSize);
                }
            }

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

        private IEnumerable<Person> FilterPage(IEnumerable<Person> persons, bool descOrder, int? pageNumber, int? pageSize)
        {
            int skipCount = pageNumber.Value * pageSize.Value;

            if (descOrder)
            {
                return persons.OrderByDescending(p => p.AskDate).Skip(skipCount).Take(pageSize.Value);
            }
            else
            {
                return persons.OrderBy(p => p.AskDate).Skip(skipCount).Take(pageSize.Value);
            }
        }
    }
}
