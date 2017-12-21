namespace Stubbl.Api.Common.CloudflareApi
{
   using Common.CloudflareApi.DnsRecords;

   public class CloudflareApi : ICloudflareApi
   {
      public CloudflareApi(IDnsRecords dnsRecords)
      {
         DnsRecords = dnsRecords;
      }

      public IDnsRecords DnsRecords { get; }
   }
}
