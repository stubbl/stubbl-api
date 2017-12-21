namespace Stubbl.Api.Core.Caching
{
   using Autofac;
   using Common.Caching;
   using Common.Caching.Memory;

   public class CachingModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterType<MemoryCache>()
            .As<ICache>()
            .SingleInstance();

         builder.RegisterType<CacheKey>()
            .As<ICacheKey>()
            .SingleInstance();
      }
   }
}