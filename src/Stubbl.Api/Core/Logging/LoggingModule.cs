namespace Stubbl.Api.Core.Logging
{
   using Autofac;
   using Microsoft.Extensions.Logging;

   public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventIdAccessor>()
                .As<IEventIdAccessor>()
                .InstancePerLifetimeScope();
        }
    }
}