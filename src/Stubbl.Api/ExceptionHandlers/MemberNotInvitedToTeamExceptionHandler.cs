namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberNotInvitedToTeam.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberNotInvitedToTeam.Version1;

   public class MemberNotInvitedToTeamExceptionHandler : IExceptionHandler<MemberNotInvitedToTeamException>
   {
      public async Task HandleAsync(HttpContext context, MemberNotInvitedToTeamException exception)
      {
         var response = new MemberNotInvitedToTeamResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
