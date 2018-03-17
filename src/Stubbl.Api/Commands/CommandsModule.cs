using Autofac;
using Gunnsoft.Cqs.Commands;

namespace Stubbl.Api.Commands
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacCommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerDependency();
        }
    }
}