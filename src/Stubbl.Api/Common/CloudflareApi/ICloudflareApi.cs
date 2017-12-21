namespace Stubbl.Api.Common.CloudflareApi
{
   using DnsRecords;

   public interface ICloudflareApi
   {
      IDnsRecords DnsRecords { get; }
   }
}
