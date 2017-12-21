namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.MemberAlreadyAddedToTeam.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberAlreadyAddedToTeam.Version1;

   public class MemberAlreadyAddedToTeamExceptionHandler : IExceptionHandler<MemberAlreadyAddedToTeamException>
   {
      public async Task HandleAsync(HttpContext context, MemberAlreadyAddedToTeamException exception)
      {
         var response = new MemberAlreadyAddedToTeamResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
