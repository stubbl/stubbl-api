namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.MemberCannotManageMembers.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotManageMembers.Version1;

   public class MemberCannotManageMembersExceptionHandler : IExceptionHandler<MemberCannotManageMembersException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotManageMembersException exception)
      {
         var response = new MemberCannotManageMembersResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
