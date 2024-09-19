using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Managers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class CollectionWriter : BaseWriter, ICollectionWriter
    {
        private readonly ILogger _logger;
        private readonly ICollectionRepository _repository;

        public CollectionWriter(ILoggerFactory loggerFactory, IMemoryCache memoryCache, ICollectionRepository repository)
            : base(loggerFactory, memoryCache)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _repository = repository;
        }

        public async Task<AccountType> AddAccountTypeAsync(AccountType imput)
        {
            var accountType = await _repository.AddAccountTypeAsync(imput).ConfigureAwait(false);

            ClearCache<CollectionManager, AccountType>();

            return accountType;
        }
    }
}
