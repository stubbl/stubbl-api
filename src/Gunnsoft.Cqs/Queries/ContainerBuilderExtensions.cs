using Autofac;

namespace Gunnsoft.Cqs.Queries
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddQueryDispatcher(this ContainerBuilder extended)
        {
            extended.RegisterType<AutofacQueryDispatcher>()
                .As<IQueryDispatcher>()
                .InstancePerDependency();

            return extended;
        }
    }
}
