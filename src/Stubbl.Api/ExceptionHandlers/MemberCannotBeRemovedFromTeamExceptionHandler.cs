namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using Common.ExceptionHandlers;
   using Core.Exceptions.MemberCannotBeRemovedFromTeam.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotBeRemovedFromTeam.Version1;

   public class MemberCannotBeRemovedFromTeamExceptionHandler : IExceptionHandler<MemberCannotBeRemovedFromTeamException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotBeRemovedFromTeamException exception)
      {
         var response = new MemberCannotBeRemovedFromTeamResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
