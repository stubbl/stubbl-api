namespace Stubbl.Api.Common.CloudflareApi
{
   public class PostRequest
   {
      public PostRequest(string pathAndQueryString)
         : this(pathAndQueryString, null)
      {
      }

      public PostRequest(string pathAndQueryString, object content)
      {
         PathAndQueryString = pathAndQueryString;
         Content = content;
      }

      public object Content { get; }
      public string PathAndQueryString { get; }
   }
}
