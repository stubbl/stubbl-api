namespace CodeContrib.CloudflareApi
{
   using DnsRecords;

   public class CloudflareApi : ICloudflareApi
   {
      public CloudflareApi(IDnsRecords dnsRecords)
      {
         DnsRecords = dnsRecords;
      }

      public IDnsRecords DnsRecords { get; }
   }
}
