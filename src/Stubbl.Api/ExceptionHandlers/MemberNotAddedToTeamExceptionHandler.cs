namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberNotAddedToTeam.Version1;

   public class MemberNotAddedToTeamExceptionHandler : IExceptionHandler<MemberNotAddedToTeamException>
   {
      public async Task HandleAsync(HttpContext context, MemberNotAddedToTeamException exception)
      {
         var response = new MemberNotAddedToTeamResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
