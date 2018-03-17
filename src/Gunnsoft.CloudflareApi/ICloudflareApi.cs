using Gunnsoft.CloudflareApi.DnsRecords;

namespace Gunnsoft.CloudflareApi
{
    public interface ICloudflareApi
    {
        IDnsRecords DnsRecords { get; }
    }
}