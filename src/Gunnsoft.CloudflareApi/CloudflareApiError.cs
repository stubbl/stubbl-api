namespace Gunnsoft.CloudflareApi
{
   public class CloudflareApiError
   {
      public CloudflareApiError(string code, string message)
      {
         Code = code;
         Message = message;
      }

      public string Code { get; }
      public string Message { get; }
   }
}
