namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Reflection;
   using Autofac;
   using Autofac.Core;
   using Gunnsoft.Cqs.CommandHandlers;
   using Module = Autofac.Module;

   public class CommandHandlersModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterAssemblyTypes(ThisAssembly)
            .As(t => t.GetInterfaces()
               .Where(i => i.IsClosedTypeOf(typeof(ICommandHandler<,>)))
               .Select(i => new KeyedService("CommandHandler", i)))
            .InstancePerDependency();

         builder.RegisterGenericDecorator
            (
               typeof(LoggingCommandHandlerDecorator<,>),
               typeof(ICommandHandler<,>),
               "CommandHandler",
               "LoggingCommandHandler"
            )
            .InstancePerDependency();

         builder.RegisterGenericDecorator
            (
               typeof(RetryCommandHandlerDecorator<,>),
               typeof(ICommandHandler<,>),
               "LoggingCommandHandler"
            )
            .InstancePerDependency();
      }
   }
}