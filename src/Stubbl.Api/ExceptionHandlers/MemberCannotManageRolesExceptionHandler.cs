namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.MemberCannotManageRoles.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotManageRoles.Version1;

   public class MemberCannotManageRolesExceptionHandler : IExceptionHandler<MemberCannotManageRolesException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotManageRolesException exception)
      {
         var response = new MemberCannotManageRolesResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
