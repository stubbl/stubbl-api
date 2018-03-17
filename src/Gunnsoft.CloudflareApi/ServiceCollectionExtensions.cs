using System.Net.Http;
using Gunnsoft.CloudflareApi.DnsRecords;
using Microsoft.Extensions.DependencyInjection;

namespace Gunnsoft.CloudflareApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudflareApi(this IServiceCollection extended,
            CloudflareApiSettings cloudflareApiSettings)
        {
            extended.AddTransient(sp =>
                new CloudflareApiHttpClient(cloudflareApiSettings, sp.GetRequiredService<HttpClient>()));
            extended.AddTransient<ICloudflareApi, CloudflareApi>();
            extended.AddTransient<IDnsRecords, HttpClientDnsRecords>();

            return extended;
        }
    }
}