using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsRepository repository;
        private readonly ILog logger;
        private readonly IMessageBus messageBus;

        private readonly ConcurrentDictionary<int, Person> personsById;
        private readonly ConcurrentDictionary<string, Person> personsByName;

        public PersonsManager(IPersonsRepository personsRepository, ILog logger, IMessageBus messageBus)
        {
            this.repository = personsRepository;
            this.logger = logger;
            this.messageBus = messageBus;

            this.personsById = new ConcurrentDictionary<int, Person>();
            this.personsByName = new ConcurrentDictionary<string, Person>(StringComparer.OrdinalIgnoreCase);

            this.messageBus.Attach(typeof(Person), this.HandleMessage);
        }

        public IEnumerable<Person> GetAllPersons(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            IEnumerable<Person> persons = this.personsByName.Values;

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                persons = this.FilterPage(persons, descOrder, pageNumber, pageSize);

                if (persons.Count() < pageSize.Value)
                {
                    IEnumerable<Person> dbPersons = this.GetAll(descOrder, pageNumber, pageSize);
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
                Person newPerson = this.repository.GetById(newId);
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
                    Person newPerson = this.repository.GetByName(newName);
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
                this.logger.Error($"Person with name '{name}' not found.");
                return null;
            }
            catch (Exception ex)
            {
                this.logger.Error($"Error in getting the person by name '{name}'", ex);
                return null;
            }
        }

        public PersonStatistics GetStatictics()
        {
            PersonStatistics statistics = this.repository.GetStatictics();
            return statistics;
        }

        private IEnumerable<Person> GetAll(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            IQueryable<Person> query = this.repository.GetAll().AsQueryable();

            if (descOrder)
            {
                query = query.OrderByDescending(p => p.AskDate);
            }
            else
            {
                query = query.OrderBy(p => p.AskDate);
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skipCount = pageSize.Value * pageNumber.Value;
                query = query.Skip(skipCount).Take(pageSize.Value);
            }

            IEnumerable<Person> persons = query.ToList();
            return persons;
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

        private void HandleMessage(Message message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Data is Person person)
            {
                if (message.ActionType == MessageActionType.Add ||
                   (message.ActionType == MessageActionType.Update && this.personsById.ContainsKey(person.Id)))
                {
                    this.personsById.AddOrUpdate(person.Id, (id) => person, (updateId, existingPerson) => person);
                }

                if (message.ActionType == MessageActionType.Add ||
                   (message.ActionType == MessageActionType.Update && this.personsByName.ContainsKey(person.Username)))
                {
                    this.personsByName.AddOrUpdate(person.Username, (id) => person, (updateId, existingPerson) => person);
                }
            }
        }
    }
}
