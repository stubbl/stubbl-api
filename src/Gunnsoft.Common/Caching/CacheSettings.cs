using System;

namespace Gunnsoft.Common.Caching
{
    public class CacheSettings
    {
        static CacheSettings()
        {
            Default = new CacheSettings
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
        }

        public static CacheSettings Default { get; set; }
        public TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }
}