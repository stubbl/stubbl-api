using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddIdentityIdAccessor(this ContainerBuilder extended)
        {
            extended.Register<IIdentityIdAccessor>(cc =>
                {
                    var hostingEnvironment = cc.Resolve<IHostingEnvironment>();

                    if (hostingEnvironment.IsDevelopment())
                    {
                        return new HeaderIdentityIdAccessor(cc.Resolve<IHttpContextAccessor>());
                    }

                    return new ClaimsIdentityIdAccessor(cc.Resolve<IHttpContextAccessor>());
                })
                .As<IIdentityIdAccessor>()
                .InstancePerLifetimeScope();

            return extended;
        }
    }
}
