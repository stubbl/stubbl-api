using Autofac;

namespace Stubbl.Api.Authentication
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddMongoUserAccessor(this ContainerBuilder extended)
        {
            extended.RegisterType<MongoAuthenticatedUserAccessor>()
                .As<IAuthenticatedUserAccessor>()
                .InstancePerLifetimeScope();

            return extended;
        }
    }
}
