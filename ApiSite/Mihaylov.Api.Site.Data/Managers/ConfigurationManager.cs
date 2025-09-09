using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class ConfigurationManager : BaseManager, IConfigurationManager
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationManager(ILoggerFactory loggerFactory, IMemoryCache memoryCache,
            IConfigurationRepository repository)
            : base(loggerFactory, memoryCache)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DefaultFilter>> GetAllDefaultFiltersAsync()
        {
            var filters = await GetDataListAsync(_repository.GetAllDefaultFiltersAsync).ConfigureAwait(false);
            return filters;
        }

        public async Task<DefaultFilter> GetDefaultFilterAsync()
        {
            var filter = await GetDataAsync(_repository.GetDefaultFilterAsync, typeof(DefaultFilter).Name).ConfigureAwait(false);
            return filter;
        }
    }
}
