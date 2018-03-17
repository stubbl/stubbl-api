using Autofac;
using Gunnsoft.Common.Caching;
using Gunnsoft.Common.Caching.Memory;

namespace Stubbl.Api.Caching
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCaching(this ContainerBuilder extended)
        {
            extended.RegisterType<MemoryCache>()
                .As<ICache>()
                .SingleInstance();

            extended.RegisterType<CacheKey>()
                .As<ICacheKey>()
                .SingleInstance();

            return extended;
        }
    }
}