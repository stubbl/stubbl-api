namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.StubNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.StubNotFound.Version1;

   public class StubNotFoundExceptionHandler : IExceptionHandler<StubNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, StubNotFoundException exception)
      {
         var response = new StubNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
