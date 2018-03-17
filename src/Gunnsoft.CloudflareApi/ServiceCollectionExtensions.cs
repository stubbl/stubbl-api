namespace Gunnsoft.CloudflareApi
{
    using DnsRecords;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Http;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudflareApi(this IServiceCollection extended, CloudflareApiSettings cloudflareApiSettings)
        {
            extended.AddTransient(sp => new CloudflareApiHttpClient(cloudflareApiSettings, sp.GetRequiredService<HttpClient>()));
            extended.AddTransient<ICloudflareApi, CloudflareApi>();
            extended.AddTransient<IDnsRecords, HttpClientDnsRecords>();

            return extended;
        }
    }
}
