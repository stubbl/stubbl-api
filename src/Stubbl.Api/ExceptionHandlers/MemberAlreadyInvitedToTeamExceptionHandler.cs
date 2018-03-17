using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberAlreadyInvitedToTeam.Version1;
using Stubbl.Api.Models.MemberAlreadyInvitedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberAlreadyInvitedToTeamExceptionHandler : IExceptionHandler<MemberAlreadyInvitedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, MemberAlreadyInvitedToTeamException exception)
        {
            var response = new MemberAlreadyInvitedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}