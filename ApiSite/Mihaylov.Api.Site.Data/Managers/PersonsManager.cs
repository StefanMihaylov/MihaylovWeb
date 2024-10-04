using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Repositories;

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
            return _repository.GetAllPersonsAsync(request);
        }

        public Task<Person> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

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

        public Task<PersonStatistics> GetStaticticsAsync()
        {
            return _repository.GetStaticticsAsync();
        }
    }
}
