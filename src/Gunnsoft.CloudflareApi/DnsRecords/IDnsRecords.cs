namespace Gunnsoft.CloudflareApi.DnsRecords
{
   using System.Threading;
   using System.Threading.Tasks;
   using DnsRecords.CreateDnsRecord;
   using DnsRecords.DeleteDnsRecord;
   using DnsRecords.ListDnsRecords;

   public interface IDnsRecords
   {
      Task<CreateDnsRecordResponse> CreateAsync(CreateDnsRecordRequest request, CancellationToken cancellationToken);
      Task DeleteAsync(DeleteDnsRecordRequest request, CancellationToken cancellationToken);
      Task<ListDnsRecordsResponse> ListAsync(ListDnsRecordsRequest request, CancellationToken cancellationToken);
   }
}
