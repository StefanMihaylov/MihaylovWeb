using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsRepository _repository;
        private readonly ILogger _logger;

        public PersonsManager(IPersonsRepository personsRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _repository = personsRepository;
        }

        public Task<Grid<Person>> GetAllPersonsAsync(GridRequest request)
        {
            var persons = _repository.GetAllPersonsAsync(request);
            return persons;
        }

        //public Person GetById(long id)
        //{
        //    Person person = this.personsById.GetOrAdd(id, (newId) =>
        //    {
        //        Person newPerson = this._repository.GetByIdAsync(newId).ConfigureAwait(false).GetAwaiter().GetResult();
        //        if (newPerson == null)
        //        {
        //            throw new ApplicationException($"Person with Id: {newId} was not found");
        //        }

        //        return newPerson;
        //    });

        //    return person;
        //}

        //public Person GetByName(string name)
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        string key = name.Trim();
        //        Person person = this.personsByName.GetOrAdd(key, (newName) =>
        //        {
        //            Person newPerson = this._repository.GetByAccoutUserNameAsync(newName).ConfigureAwait(false).GetAwaiter().GetResult();
        //            if (newPerson == null)
        //            {
        //                throw new ApplicationException($"Person with name: {newName} was not found");
        //            }

        //            return newPerson;
        //        });

        //        return person;
        //    }
        //    catch (ApplicationException)
        //    {
        //        this._logger.LogError($"Person with name '{name}' not found.");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        this._logger.LogError(ex, $"Error in getting the person by name '{name}'");
        //        return null;
        //    }
        //}

        //public async Task<PersonStatistics> GetStaticticsAsync()
        //{
        //    PersonStatistics statistics = await this._repository.GetStaticticsAsync().ConfigureAwait(false);
        //    return statistics;
        //}
    }
}
