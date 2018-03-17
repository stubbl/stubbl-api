using System.Net.Http;
using Autofac;
using Gunnsoft.CloudflareApi.DnsRecords;

namespace Gunnsoft.CloudflareApi
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCloudflareApi(this ContainerBuilder extended,
            CloudflareApiSettings cloudflareApiSettings)
        {
            extended.Register(cc =>
                new CloudflareApiHttpClient(cloudflareApiSettings, cc.Resolve<HttpClient>()))
                .AsSelf()
                .InstancePerDependency();
            extended.RegisterType<CloudflareApi>()
                .As<ICloudflareApi>()
                .InstancePerDependency();
            extended.RegisterType<HttpClientDnsRecords>()
                .As<IDnsRecords>()
                .InstancePerDependency();

            return extended;
        }
    }
}