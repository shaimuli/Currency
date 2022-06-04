using System;
using System.Runtime.Caching;

namespace server.Helper
{
    public class CurrencyCache
    {
        private ObjectCache cache;
        private CacheItemPolicy policy;
        //add parameter to config
        private double duration = 60;
        private string cacheName = "CacheToCurrencies";

        public CurrencyCache()
        {
            cache = MemoryCache.Default;
            policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(duration)
            };
        }
        public void Set(CurrenciesList json)
        {
            if (json != null)
            {
                if (duration > 0) cache.Set(cacheName, json, policy);
            }
            else if (cache.Contains(cacheName))
            {
                cache.Remove(cacheName);
            }
        }
        public CurrenciesList Get()
        {
            if (cache.Contains(cacheName))
            {
                if (duration > 0) return cache[cacheName] as CurrenciesList;
            }
            return null;
        }
        public bool Exists()
        {
            return cache.Contains(cacheName);
        }
    }
}
