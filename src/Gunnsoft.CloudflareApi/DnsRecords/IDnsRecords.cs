using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.CloudflareApi.DnsRecords.CreateDnsRecord;
using Gunnsoft.CloudflareApi.DnsRecords.DeleteDnsRecord;
using Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords;

namespace Gunnsoft.CloudflareApi.DnsRecords
{
    public interface IDnsRecords
    {
        Task<CreateDnsRecordResponse> CreateAsync(CreateDnsRecordRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(DeleteDnsRecordRequest request, CancellationToken cancellationToken);
        Task<ListDnsRecordsResponse> ListAsync(ListDnsRecordsRequest request, CancellationToken cancellationToken);
    }
}