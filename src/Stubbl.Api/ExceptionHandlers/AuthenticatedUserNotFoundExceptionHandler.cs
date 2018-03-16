namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.AuthenticatedUserNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.AuthenticatedUserNotFound.Version1;

   public class AuthenticatedUserNotFoundExceptionHandler : IExceptionHandler<AuthenticatedUserNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, AuthenticatedUserNotFoundException exception)
      {
         var response = new AuthenticatedUserNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Unauthorized, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
