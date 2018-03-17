namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Core.Exceptions.InvitationNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.InvitationNotFound.Version1;

   public class InvitationNotFoundExceptionHandler : IExceptionHandler<InvitationNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, InvitationNotFoundException exception)
      {
         var response = new InvitationNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
