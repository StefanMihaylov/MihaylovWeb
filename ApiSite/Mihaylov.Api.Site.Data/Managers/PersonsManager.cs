using System.Collections.Generic;
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

        public Task<Person> GetPersonAsync(long id)
        {
            return _repository.GetPersonAsync(id);
        }

        public Task<Account> GetAccountAsync(long id)
        {
            return _repository.GetAccountAsync(id);
        }

        public Task<UpdateAccounts> GetAllAccountsForUpdateAsync(int? batchSize)
        {
            return _repository.GetAllAccountsForUpdateAsync(batchSize);
        }

        public Task<PersonStatistics> GetStaticticsAsync()
        {
            return _repository.GetStaticticsAsync();
        }
    }
}
