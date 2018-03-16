namespace CodeContrib.CloudflareApi
{
   using DnsRecords;

   public interface ICloudflareApi
   {
      IDnsRecords DnsRecords { get; }
   }
}
