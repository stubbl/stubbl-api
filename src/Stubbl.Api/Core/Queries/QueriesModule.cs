namespace Stubbl.Api.Core.Queries
{
   using Autofac;
   using Common.Queries;

   public class QueriesModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterType<AutofacQueryDispatcher>()
            .As<IQueryDispatcher>()
            .InstancePerDependency();
      }
   }
}