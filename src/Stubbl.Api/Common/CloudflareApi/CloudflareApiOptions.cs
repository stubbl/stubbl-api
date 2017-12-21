namespace Stubbl.Api.Common.CloudflareApi
{
   public class CloudflareApiOptions
   {
      public CloudflareApiOptions(string baseUrl, string authenticationKey, string authenticationEmailAddress)
      {
         BaseUrl = baseUrl;
         AuthenticationKey = authenticationKey;
         AuthenticationEmailAddress = authenticationEmailAddress;
      }

      public string BaseUrl { get; }
      public string AuthenticationEmailAddress { get; }
      public string AuthenticationKey { get; }
   }
}
