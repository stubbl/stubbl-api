using System.Linq;
using Autofac;
using Autofac.Core;
using Gunnsoft.Cqs.QueryHandlers;

namespace Stubbl.Api.QueryHandlers
{
    public class QueryHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                    .Select(i => new KeyedService("QueryHandler", i)))
                .InstancePerDependency();

            builder.RegisterGenericDecorator
                (
                    typeof(LoggingQueryHandlerDecorator<,>),
                    typeof(IQueryHandler<,>),
                    "QueryHandler",
                    "LoggingQueryHandler"
                )
                .InstancePerDependency();

            builder.RegisterGenericDecorator
                (
                    typeof(RetryQueryHandlerDecorator<,>),
                    typeof(IQueryHandler<,>),
                    "LoggingQueryHandler"
                )
                .InstancePerDependency();
        }
    }
}