using System;
using System.Threading.Tasks;

namespace Gunnsoft.Common.Caching
{
    public interface ICache
    {
        TValue GetOrSet<TValue>(object key, Func<TValue> factory, CacheSettings cacheOptions = null);
        Task<TValue> GetOrSetAsync<TValue>(object key, Func<Task<TValue>> factory, CacheSettings cacheOptions = null);
        void Remove(object key);
    }
}