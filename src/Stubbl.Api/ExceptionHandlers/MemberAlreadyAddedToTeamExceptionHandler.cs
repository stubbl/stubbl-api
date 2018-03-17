using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Models.MemberAlreadyAddedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberAlreadyAddedToTeamExceptionHandler : IExceptionHandler<MemberAlreadyAddedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, MemberAlreadyAddedToTeamException exception)
        {
            var response = new MemberAlreadyAddedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}