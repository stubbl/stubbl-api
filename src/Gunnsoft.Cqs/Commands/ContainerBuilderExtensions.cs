using Autofac;

namespace Gunnsoft.Cqs.Commands
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCommandDispatcher(this ContainerBuilder extended)
        {
            extended.RegisterType<AutofacCommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerDependency();

            return extended;
        }
    }
}
