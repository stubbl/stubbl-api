namespace Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords
{
    public class ListDnsRecordsRequest
    {
        public ListDnsRecordsRequest(string zoneId)
        {
            ZoneId = zoneId;
        }

        public string Content { get; set; }
        public string Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public SortField? SortField { get; set; }
        public SortDirection? SortDirection { get; set; }
        public DnsRecordType? Type { get; set; }
        public Match? Match { get; set; }
        public string ZoneId { get; set; }
    }
}