using Autofac;

namespace Gunnsoft.Cqs.Events
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddEventDispatcher(this ContainerBuilder extended)
        {
            extended.RegisterType<AutofacEventDispatcher>()
                .As<IEventDispatcher>()
                .InstancePerDependency();

            return extended;
        }
    }
}
