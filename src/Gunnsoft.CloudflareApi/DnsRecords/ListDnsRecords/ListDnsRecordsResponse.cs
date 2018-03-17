using System.Collections.Generic;

namespace Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords
{
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