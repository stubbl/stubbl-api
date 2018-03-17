using System.Linq;
using Autofac;
using Gunnsoft.Api.ExceptionHandlers;
using Stubbl.Api.Versioning;

namespace Stubbl.Api.ExceptionHandlers
{
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