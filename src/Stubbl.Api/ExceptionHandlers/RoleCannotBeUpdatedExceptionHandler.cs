namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Core.Exceptions.RoleCannotBeUpdated.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.RoleCannotBeUpdated.Version1;

   public class RoleCannotBeUpdatedExceptionHandler : IExceptionHandler<RoleCannotBeUpdatedException>
   {
      public async Task HandleAsync(HttpContext context, RoleCannotBeUpdatedException exception)
      {
         var response = new RoleCannotBeUpdatedResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
