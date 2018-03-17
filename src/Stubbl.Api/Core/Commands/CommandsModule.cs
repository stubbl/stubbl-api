namespace Stubbl.Api.Core.Commands
{
   using Gunnsoft.Cqs.Commands;
   using Autofac;

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