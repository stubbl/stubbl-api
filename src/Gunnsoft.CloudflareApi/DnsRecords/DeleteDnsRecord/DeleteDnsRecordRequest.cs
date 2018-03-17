namespace Gunnsoft.CloudflareApi.DnsRecords.DeleteDnsRecord
{
   public class DeleteDnsRecordRequest
   {
      public DeleteDnsRecordRequest(string zoneId, string dnsRecordId)
      {
         ZoneId = zoneId;
         DnsRecordId = dnsRecordId;
      }

      public string DnsRecordId { get; }
      public string ZoneId { get; }
   }
}
