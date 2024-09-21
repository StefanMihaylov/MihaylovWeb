using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class CollectionManager : BaseManager, ICollectionManager
    {
        private readonly ICollectionRepository _repository;

        public CollectionManager(ILoggerFactory loggerFactory, IMemoryCache memoryCache, 
            ICollectionRepository repository) 
            : base(loggerFactory, memoryCache)
        {
           // ParameterValidation.IsNotNull(repository, nameof(repository));

            _repository = repository;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countries = await GetDataListAsync(_repository.GetAllCountriesAsync).ConfigureAwait(false);
            return countries;
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            var countries = await _repository.GetCountryByNameAsync(name).ConfigureAwait(false);
            return countries;
        }

        public async Task<IEnumerable<CountryState>> GetAllStatesByCountryIdAsync(int countryId)
        {
            var stateDict = await GetDataAsync(_repository.GetAllStatesAsync, typeof(CountryState).Name).ConfigureAwait(false);
            if (stateDict.TryGetValue(countryId, out IEnumerable<CountryState> states))
            {
                return states;
            }
            else
            {
                return new List<CountryState>();
            }
        }

        public async Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync()
        {
            var ethnicities = await GetDataListAsync(_repository.GetAllEthnicitiesAsync).ConfigureAwait(false);
            return ethnicities;
        }

        public async Task<IEnumerable<Orientation>> GetAllOrientationsAsync()
        {
            var orientations = await GetDataListAsync(_repository.GetAllOrientationsAsync).ConfigureAwait(false);
            return orientations;
        }

        public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync()
        {
            var accountTypes = await GetDataListAsync(_repository.GetAllAccountTypesAsync).ConfigureAwait(false);
            return accountTypes;
        }

        public async Task<IEnumerable<AccountStatus>> GetAllAccountStatesAsync()
        {
            var answerTypes = await GetDataListAsync(_repository.GetAllAccountStatesAsync).ConfigureAwait(false);
            return answerTypes;
        }
    }
}
