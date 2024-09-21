using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Data.Managers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public abstract class BaseWriter
    {
        protected readonly ILogger _logger;
        protected readonly IMemoryCache _memoryCache;

        public BaseWriter(ILoggerFactory loggerFactory, IMemoryCache memoryCache)
        {
           // ParameterValidation.IsNotNull(loggerFactory, nameof(loggerFactory));
           // ParameterValidation.IsNotNull(memoryCache, nameof(memoryCache));

            _logger = loggerFactory.CreateLogger(GetType().Name);
            _memoryCache = memoryCache;
        }

        protected void ClearCache<M, T>() where M : BaseManager
        {
            var key = BaseManager.GetCacheKeyName(typeof(M).Name, typeof(T).Name);
            _memoryCache.Remove(key);
        }
    }
}
