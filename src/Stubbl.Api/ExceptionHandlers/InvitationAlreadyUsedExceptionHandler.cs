namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.InvitationAlreadyUsed.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.InvitationAlreadyUsed.Version1;

   public class InvitationAlreadyUsedExceptionHandler : IExceptionHandler<InvitationAlreadyUsedException>
   {
      public async Task HandleAsync(HttpContext context, InvitationAlreadyUsedException exception)
      {
         var response = new InvitationAlreadyUsedResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
