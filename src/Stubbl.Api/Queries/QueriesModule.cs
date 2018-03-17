using Autofac;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries
{
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