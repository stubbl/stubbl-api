﻿using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace Gunnsoft.Cqs.QueryHandlers
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddQueryHandlerDecorators(this ContainerBuilder extended)
        {
            extended.RegisterGenericDecorator
                (
                    typeof(LoggingQueryHandlerDecorator<,>),
                    typeof(IQueryHandler<,>),
                    "QueryHandler",
                    "LoggingQueryHandler"
                )
                .InstancePerDependency();

            extended.RegisterGenericDecorator
                (
                    typeof(RetryQueryHandlerDecorator<,>),
                    typeof(IQueryHandler<,>),
                    "LoggingQueryHandler"
                )
                .InstancePerDependency();

            return extended;
        }

        public static ContainerBuilder AddQueryHandlers(this ContainerBuilder extended, Assembly assembly)
        {
            extended.RegisterAssemblyTypes(assembly)
                .As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                    .Select(i => new KeyedService("QueryHandler", i)))
                .InstancePerDependency();

            return extended;
        }
    }
}