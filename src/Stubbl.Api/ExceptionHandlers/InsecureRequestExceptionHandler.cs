namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Middleware.SecureRequests;
   using Models.Error.Version1;

   public class InsecureRequestExceptionHandler : IExceptionHandler<InsecureRequestException>
   {
      public async Task HandleAsync(HttpContext context, InsecureRequestException exception)
      {
         var response = new ErrorResponse("InsecureRequest", "Requests must be made using HTTPS.");

         await context.Response.WriteJsonAsync(HttpStatusCode.HttpVersionNotSupported, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
