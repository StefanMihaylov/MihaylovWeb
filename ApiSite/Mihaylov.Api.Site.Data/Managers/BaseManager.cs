using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Common.Validations;

namespace Mihaylov.Api.Site.Data.Managers
{
    public abstract class BaseManager
    {
        private const int CACHE_DURATION = 30;
        protected readonly TimeSpan _cache_duration = TimeSpan.FromMinutes(CACHE_DURATION);

        protected readonly ILogger _logger;
        protected readonly IMemoryCache _memoryCache;
        private readonly string _managerName;

        public BaseManager(ILoggerFactory loggerFactory, IMemoryCache memoryCache)
        {
            ParameterValidation.IsNotNull(loggerFactory, nameof(loggerFactory));
            ParameterValidation.IsNotNull(memoryCache, nameof(memoryCache));

            _managerName = this.GetType().Name;
            _logger = loggerFactory.CreateLogger(_managerName);
            _memoryCache = memoryCache;
        }
        public static string GetCacheKeyName(string managerName, string keySuffix)
        {
            return $"Site_{managerName}_Get_{keySuffix}";
        }

        protected Task<IEnumerable<T>> GetDataListAsync<T>(Func<Task<IEnumerable<T>>> feedAsync)
        {
            return GetDataAsync(feedAsync, typeof(T).Name);
        }

        protected async Task<T> GetDataAsync<T>(Func<Task<T>> feedAsync, string keySuffix)
        {
            string key = GetCacheKeyName(_managerName, keySuffix);
            if (!_memoryCache.TryGetValue(key, out T data))
            {
                data = await feedAsync().ConfigureAwait(false);

                _memoryCache.Set(key, data, _cache_duration);
            }

            return data;
        }
    }
}
