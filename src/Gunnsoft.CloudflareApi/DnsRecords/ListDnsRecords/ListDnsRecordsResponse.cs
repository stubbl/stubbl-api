namespace Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords
{
   using System.Collections.Generic;

   public class ListDnsRecordsResponse
   {
      public ListDnsRecordsResponse(IReadOnlyCollection<DnsRecord> result, Paging resultInfo)
      {
         DnsRecords = result;
         Paging = resultInfo;
      }

      public IReadOnlyCollection<DnsRecord> DnsRecords { get; }
      public Paging Paging { get; }
   }
}
