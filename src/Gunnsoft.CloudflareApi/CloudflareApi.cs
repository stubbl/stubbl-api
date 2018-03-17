using Gunnsoft.CloudflareApi.DnsRecords;

namespace Gunnsoft.CloudflareApi
{
    public class CloudflareApi : ICloudflareApi
    {
        public CloudflareApi(IDnsRecords dnsRecords)
        {
            DnsRecords = dnsRecords;
        }

        public IDnsRecords DnsRecords { get; }
    }
}