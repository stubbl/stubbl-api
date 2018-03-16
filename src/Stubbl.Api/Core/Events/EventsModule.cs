namespace Stubbl.Api.Core.Events
{
   using CodeContrib.Events;
   using Autofac;

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