namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Core.Exceptions.MemberNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberNotFound.Version1;

   public class MemberNotFoundExceptionHandler : IExceptionHandler<MemberNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, MemberNotFoundException exception)
      {
         var response = new MemberNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
