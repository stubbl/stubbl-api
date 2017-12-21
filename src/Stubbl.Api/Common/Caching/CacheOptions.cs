namespace Stubbl.Api.Common.Caching
{
   using System;

   public class CacheOptions
   {
      static CacheOptions()
      {
         Default = new CacheOptions
         {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
         };
      }

      public static CacheOptions Default { get; set; }
      public TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
      public TimeSpan? SlidingExpiration { get; set; }
   }
}