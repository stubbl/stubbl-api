namespace Stubbl.Api.Modules
{
   using Autofac;
   using Common.CloudflareApi;
   using Common.CloudflareApi.DnsRecords;
   using Microsoft.Extensions.Configuration;

   public class CloudflareApiModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.Register(cc =>
            {
               var configuration = cc.Resolve<IConfiguration>();
               var configurationSection = configuration.GetSection("CloudflareApi");
               var baseUrl = configurationSection.GetValue<string>("BaseUrl");
               var authenticationKey = configurationSection.GetValue<string>("AuthenticationKey");
               var authenticationEmailAddress = configurationSection.GetValue<string>("AuthenticationEmailAddress");

               return new CloudflareApiOptions
               (
                  baseUrl,
                  authenticationKey,
                  authenticationEmailAddress
               );
            })
            .AsSelf()
            .InstancePerDependency();

         builder.RegisterType<CloudflareApiHttpClient>()
            .As<ICloudflareApiHttpClient>()
            .InstancePerDependency();

         builder.RegisterType<CloudflareApi>()
            .As<ICloudflareApi>()
            .InstancePerDependency();

         builder.RegisterType<HttpClientDnsRecords>()
            .As<IDnsRecords>()
            .InstancePerDependency();
      }
   }
}
