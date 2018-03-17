namespace Gunnsoft.Common.Caching
{
   using System;
   using System.Threading.Tasks;

   public interface ICache
   {
      TValue GetOrSet<TValue>(object key, Func<TValue> factory, CacheSettings cacheOptions = null);
      Task<TValue> GetOrSetAsync<TValue>(object key, Func<Task<TValue>> factory, CacheSettings cacheOptions = null);
      void Remove(object key);
   }
}