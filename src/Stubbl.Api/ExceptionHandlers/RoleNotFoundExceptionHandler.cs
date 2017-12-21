namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.RoleNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.RoleNotFound.Version1;

   public class RoleNotFoundExceptionHandler : IExceptionHandler<RoleNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, RoleNotFoundException exception)
      {
         var response = new RoleNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
