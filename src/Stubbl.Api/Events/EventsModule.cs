using Autofac;
using Gunnsoft.Cqs.Events;

namespace Stubbl.Api.Events
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacEventDispatcher>()
                .As<IEventDispatcher>()
                .InstancePerDependency();
        }
    }
}