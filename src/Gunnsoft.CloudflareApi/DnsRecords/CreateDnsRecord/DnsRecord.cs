namespace Gunnsoft.CloudflareApi.DnsRecords.CreateDnsRecord
{
   using System;

   public class DnsRecord
   {
      public DnsRecord(string id, DnsRecordType type, string name, string content, bool proxiable, bool proxied,
          bool locked, string zoneId, string zoneName, DateTime createdOn, DateTime modifiedOn)
      {
         Id = id;
         Type = type;
         Name = name;
         Content = content;
         IsProxiable = proxiable;
         IsProxied = proxied;
         IsLocked = locked;
         ZoneId = zoneId;
         ZoneName = zoneName;
         CreatedAt = createdOn;
         ModifiedAt = modifiedOn;
      }

      public DateTime CreatedAt { get; }
      public string Content { get; }
      public string Id { get; }
      public bool IsLocked { get; }
      public bool IsProxiable { get; }
      public bool IsProxied { get; }
      public DateTime ModifiedAt { get; }
      public string Name { get; }
      public DnsRecordType Type { get; }
      public int TimeToLive { get; }
      public string ZoneId { get; }
      public string ZoneName { get; }
   }
}
