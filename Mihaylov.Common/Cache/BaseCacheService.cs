namespace Mihaylov.Common.Cache
{
    using System;
    using System.Web;
    using System.Web.Caching;

    public abstract class BaseCacheService<T> where T : class
    {
        private string cacheId;
        private double cacheExpirationSeconds;

        public BaseCacheService(string cacheId)
            : this(cacheId, 0)
        {
        }

        public BaseCacheService(string cacheId, double cacheExpirationSeconds)
        {
            this.cacheId = cacheId;
            this.cacheExpirationSeconds = cacheExpirationSeconds;
        }

        public T GetAll(int chatRoomId)
        {
            string key = string.Format("{0}_{1}", this.cacheId, chatRoomId);
            T itemsInCache = HttpRuntime.Cache.Get(key) as T;
            T items = this.ValidateCache(itemsInCache);

            if (items == null)
            {
                items = this.GetItemsFromDataSource(chatRoomId);

                var absoluteExpiration = DateTime.UtcNow.AddSeconds(this.cacheExpirationSeconds);
                if (this.cacheExpirationSeconds == 0)
                {
                    absoluteExpiration = Cache.NoAbsoluteExpiration;
                }

                HttpRuntime.Cache.Insert(key, items, null, absoluteExpiration, Cache.NoSlidingExpiration);
            }

            return items;
        }

        //public bool IsCacheCleared()
        //{
        //    bool result = HttpRuntime.Cache.Get(this.cacheId) == null;
        //    return result;
        //}

        public void Clear(int chatRoomId)
        {
            string key = string.Format("{0}_{1}", this.cacheId, chatRoomId);
            HttpRuntime.Cache.Remove(key);
        }

        protected abstract T GetItemsFromDataSource(int chatRoomId);

        protected virtual T ValidateCache(T items)
        {
            return items;
        }
    }
}
