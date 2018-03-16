namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.TeamNotFound.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.TeamNotFound.Version1;

   public class TeamNotFoundExceptionHandler : IExceptionHandler<TeamNotFoundException>
   {
      public async Task HandleAsync(HttpContext context, TeamNotFoundException exception)
      {
         var response = new TeamNotFoundResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
