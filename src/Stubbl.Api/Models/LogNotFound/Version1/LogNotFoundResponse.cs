namespace Stubbl.Api.Models.LogNotFound.Version1
{
   using Error.Version1;

   public class LogNotFoundResponse : ErrorResponse
   {
      public LogNotFoundResponse()
         : base("LogNotFound", "The log cannot be found.")
      {
      }
   }
}