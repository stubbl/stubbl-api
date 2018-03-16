namespace Stubbl.Api.ExceptionHandlers
{
   using System.Linq;
   using Autofac;
   using CodeContrib.ExceptionHandlers;
   using Versioning;
   using Module = Autofac.Module;

   public class ExceptionHandlersModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterAssemblyTypes(ThisAssembly)
            .As(t => t.GetInterfaces()
               .Where(i => i.IsClosedTypeOf(typeof(IExceptionHandler<>))))
            .SingleInstance();

         builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(t => typeof(IDefaultExceptionHandler).IsAssignableFrom(t) && Versions.IsInVersionedNamespace(t))
            .Keyed<IDefaultExceptionHandler>(t => Versions.GetVersionFromNamespace(t))
            .SingleInstance();
      }
   }
}
