using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Managers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class ConfigurationWriter : BaseWriter, IConfigurationWriter
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationWriter(ILoggerFactory loggerFactory, IMemoryCache memoryCache, IConfigurationRepository repository)
            : base(loggerFactory, memoryCache)
        {
            _repository = repository;
        }

        public async Task<DefaultFilter> AddDefaultFilterAsync(DefaultFilter input)
        {
            var filter = await _repository.AddDefaultFilterAsync(input).ConfigureAwait(false);

            ClearCache<ConfigurationManager, DefaultFilter>();

            return filter;
        }
    }
}
