using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsRepository repository;
        private readonly ILogger logger;
       // private readonly IMessageBus messageBus;

        private readonly ConcurrentDictionary<Guid, Person> personsById;
        private readonly ConcurrentDictionary<string, Person> personsByName;

        public PersonsManager(IPersonsRepository personsRepository, ILoggerFactory loggerFactory)
        {
            this.repository = personsRepository;
            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
           // this.messageBus = messageBus;

            this.personsById = new ConcurrentDictionary<Guid, Person>();
            this.personsByName = new ConcurrentDictionary<string, Person>(StringComparer.OrdinalIgnoreCase);

            //this.messageBus.Attach(typeof(Person), this.HandleMessage);
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            IEnumerable<Person> persons = this.personsByName.Values;

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                persons = this.FilterPage(persons, descOrder, pageNumber, pageSize);

                if (persons.Count() < pageSize.Value)
                {
                    IEnumerable<Person> dbPersons = await this.repository.Search(descOrder, pageNumber, pageSize).ConfigureAwait(false);
                    foreach (var dbPerson in dbPersons)
                    {
                        this.personsByName.TryAdd(dbPerson.Username, dbPerson);
                    }

                    persons = this.FilterPage(this.personsByName.Values, descOrder, pageNumber, pageSize);
                }
            }

            return persons;
        }

        public Person GetById(Guid id)
        {
            Person person = this.personsById.GetOrAdd(id, (newId) =>
            {
                Person newPerson = this.repository.GetByIdAsync(newId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (newPerson == null)
                {
                    throw new ApplicationException($"Person with Id: {newId} was not found");
                }

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
                    Person newPerson = this.repository.GetByAccoutUserNameAsync(newName).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (newPerson == null)
                    {
                        throw new ApplicationException($"Person with name: {newName} was not found");
                    }

                    return newPerson;
                });

                return person;
            }
            catch (ApplicationException)
            {
                this.logger.LogError($"Person with name '{name}' not found.");
                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in getting the person by name '{name}'");
                return null;
            }
        }

        public async Task<PersonStatistics> GetStaticticsAsync()
        {
            PersonStatistics statistics = await this.repository.GetStaticticsAsync().ConfigureAwait(false);
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

        //private void HandleMessage(Message message)
        //{
        //    if (message == null)
        //    {
        //        return;
        //    }

        //    if (message.Data is Person person)
        //    {
        //        if (message.ActionType == MessageActionType.Add ||
        //           (message.ActionType == MessageActionType.Update && this.personsById.ContainsKey(person.Id)))
        //        {
        //            this.personsById.AddOrUpdate(person.Id, (id) => person, (updateId, existingPerson) => person);
        //        }

        //        if (message.ActionType == MessageActionType.Add ||
        //           (message.ActionType == MessageActionType.Update && this.personsByName.ContainsKey(person.Username)))
        //        {
        //            this.personsByName.AddOrUpdate(person.Username, (id) => person, (updateId, existingPerson) => person);
        //        }
        //    }
        //}
    }
}
