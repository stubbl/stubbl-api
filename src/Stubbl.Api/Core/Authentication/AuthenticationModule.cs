namespace Stubbl.Api.Core.Authentication
{
   using Autofac;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.Http;

   public class AuthenticationModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.Register<IIdentityIdAccessor>(cc =>
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

         builder.RegisterType<MongoDBAuthenticatedUserAccessor>()
            .As<IAuthenticatedUserAccessor>()
            .InstancePerLifetimeScope();
      }
   }
}