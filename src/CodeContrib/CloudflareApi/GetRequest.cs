namespace CodeContrib.CloudflareApi
{
   public class GetRequest
   {
      public GetRequest(string pathAndQueryString)
      {
         PathAndQueryString = pathAndQueryString;
      }

      public string PathAndQueryString { get; }
   }
}
