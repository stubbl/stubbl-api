namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.RoleAlreadyExists.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.RoleAlreadyExists.Version1;

   public class RoleAlreadyExistsExceptionHandler : IExceptionHandler<RoleAlreadyExistsException>
   {
      public async Task HandleAsync(HttpContext context, RoleAlreadyExistsException exception)
      {
         var response = new RoleAlreadyExistsResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
