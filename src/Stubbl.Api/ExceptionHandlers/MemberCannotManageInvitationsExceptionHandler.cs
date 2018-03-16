namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberCannotManageInvitations.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotManageInvitations.Version1;

   public class MemberCannotManageInvitationsExceptionHandler : IExceptionHandler<MemberCannotManageInvitationsException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotManageInvitationsException exception)
      {
         var response = new MemberCannotManageInvitationsResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
