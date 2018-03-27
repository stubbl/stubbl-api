using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Gunnsoft.Api.Versioning;

namespace Gunnsoft.Api.ExceptionHandlers
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddExceptionHandlers(this ContainerBuilder extended)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AddExcentionHandlers(assembly);
            }

            void AddExcentionHandlers(Assembly assembly)
            {
                extended.RegisterAssemblyTypes(assembly)
                    .As(t => t.GetInterfaces()
                        .Where(i => i.IsClosedTypeOf(typeof(IExceptionHandler<>))))
                    .SingleInstance();

                extended.RegisterAssemblyTypes(assembly)
                    .Where(t => typeof(IDefaultExceptionHandler).IsAssignableFrom(t) &&
                                Versions.IsInVersionedNamespace(t))
                    .Keyed<IDefaultExceptionHandler>(t => Versions.GetVersionFromNamespace(t))
                    .SingleInstance();
            }

            return extended;
        }
    }
}