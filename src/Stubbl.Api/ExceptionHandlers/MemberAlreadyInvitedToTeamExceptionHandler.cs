namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberAlreadyInvitedToTeam.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberAlreadyInvitedToTeam.Version1;

   public class MemberAlreadyInvitedToTeamExceptionHandler : IExceptionHandler<MemberAlreadyInvitedToTeamException>
   {
      public async Task HandleAsync(HttpContext context, MemberAlreadyInvitedToTeamException exception)
      {
         var response = new MemberAlreadyInvitedToTeamResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
