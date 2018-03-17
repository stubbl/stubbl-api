namespace Stubbl.Api.Core.EventHandlers
{
   using System.Linq;
   using Autofac;
   using Autofac.Core;
   using Gunnsoft.Cqs.EventHandlers;
   using Module = Autofac.Module;

   public class EventHandlersModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterAssemblyTypes(ThisAssembly)
            .As(t => t.GetInterfaces()
               .Where(i => i.IsClosedTypeOf(typeof(IEventHandler<>)))
               .Select(i => new KeyedService("EventHandler", i)))
            .InstancePerDependency();

         builder.RegisterGenericDecorator
            (
               typeof(LoggingEventHandlerDecorator<>),
               typeof(IEventHandler<>),
               "EventHandler",
               "LoggingEventHandler"
            )
            .InstancePerDependency();

         builder.RegisterGenericDecorator
            (
               typeof(RetryEventHandlerDecorator<>),
               typeof(IEventHandler<>),
               "LoggingEventHandler"
            )
            .InstancePerDependency();
      }
   }
}