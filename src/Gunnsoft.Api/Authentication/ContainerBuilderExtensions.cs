using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddSubAccessor(this ContainerBuilder extended)
        {
            extended.Register<ISubAccessor>(cc =>
                {
                    var hostingEnvironment = cc.Resolve<IHostingEnvironment>();

                    if (hostingEnvironment.IsDevelopment())
                    {
                        return new HeaderSubAccessor(cc.Resolve<IHttpContextAccessor>());
                    }

                    return new ClaimsSubAccessor(cc.Resolve<IHttpContextAccessor>());
                })
                .As<ISubAccessor>()
                .InstancePerLifetimeScope();

            return extended;
        }
    }
}
