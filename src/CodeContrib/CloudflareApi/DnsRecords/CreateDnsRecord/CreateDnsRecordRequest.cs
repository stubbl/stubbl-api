namespace CodeContrib.CloudflareApi.DnsRecords.CreateDnsRecord
{
   public class CreateDnsRecordRequest
   {
      public CreateDnsRecordRequest(string zoneId, string name, DnsRecordType type, string content)
      {
         ZoneId = zoneId;
         Name = name;
         Type = type;
         Content = content;

         TimeToLive = 1;
      }

      public string Content { get; }
      public string Name { get; }
      public bool Proxied { get; set; }
      public int TimeToLive { get; set; }
      public DnsRecordType Type { get; }
      public string ZoneId { get; }
   }


}
