namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Core.Exceptions.LogNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.LogNotFound.Version1;

   public class LogNotFoundExceptionHandler : IExceptionHandler<LogNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, LogNotFoundException exception)
      {
         var response = new LogNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
