namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.AuthenticatedMemberNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.AuthenticatedMemberNotFound.Version1;

   public class AuthenticatedMemberNotFoundExceptionHandler : IExceptionHandler<AuthenticatedMemberNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, AuthenticatedMemberNotFoundException exception)
      {
         var response = new AuthenticatedMemberNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Unauthorized, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
