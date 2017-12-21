namespace Stubbl.Api.Common.Caching.Memory
{
   using System;
   using System.Threading.Tasks;
   using Microsoft.Extensions.Caching.Memory;

   public class MemoryCache : ICache
   {
      private readonly IMemoryCache _memoryCache;

      public MemoryCache(IMemoryCache memoryCache)
      {
         _memoryCache = memoryCache;
      }

      public TValue GetOrSet<TValue>(object key, Func<TValue> factory,
         CacheOptions cacheOptions = null)
      {
         return _memoryCache.GetOrCreate(key, ce =>
         {
            cacheOptions = cacheOptions ?? CacheOptions.Default;

            if (cacheOptions == null)
            {
               return factory();
            }

            ce.AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow;
            ce.SlidingExpiration = cacheOptions.SlidingExpiration;

            return factory();
         });
      }

      public async Task<TValue> GetOrSetAsync<TValue>(object key, Func<Task<TValue>> factory,
         CacheOptions cacheOptions = null)
      {
         return await _memoryCache.GetOrCreateAsync(key, ce =>
         {
            cacheOptions = cacheOptions ?? CacheOptions.Default;

            if (cacheOptions == null)
            {
               return factory();
            }

            ce.AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow;
            ce.SlidingExpiration = cacheOptions.SlidingExpiration;

            return factory();
         });
      }

      public void Remove(object key)
      {
         _memoryCache.Remove(key);
      }
   }
}