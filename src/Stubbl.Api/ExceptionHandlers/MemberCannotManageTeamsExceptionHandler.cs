namespace Stubbl.Api.ExceptionHandlers
{
   using System.Net;
   using System.Threading.Tasks;
   using CodeContrib.ExceptionHandlers;
   using Core.Exceptions.MemberCannotManageTeams.Version1;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Http;
   using Models.MemberCannotManageTeams.Version1;

   public class MemberCannotManageTeamsExceptionHandler : IExceptionHandler<MemberCannotManageTeamsException>
   {
      public async Task HandleAsync(HttpContext context, MemberCannotManageTeamsException exception)
      {
         var response = new MemberCannotManageTeamsResponse();

         await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
