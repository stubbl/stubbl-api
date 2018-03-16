namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Authentication;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.UnknownIdentityId.Version1;

   public class UnknownIdentityIdExceptionHandler : IExceptionHandler<UnknownIdentityIdException>
   {
      public async Task HandleAsync(HttpContext context, UnknownIdentityIdException exception)
      {
         var response = new UnknownIdentityIdResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Unauthorized, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
