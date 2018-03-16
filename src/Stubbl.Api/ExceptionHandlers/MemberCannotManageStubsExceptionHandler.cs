namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberCannotManageStubs.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotManageStubs.Version1;

   public class MemberCannotManageStubsExceptionHandler : IExceptionHandler<MemberCannotManageStubsException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotManageStubsException exception)
      {
         var response = new MemberCannotManageStubsResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
