using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberNotInvitedToTeam.Version1;
using Stubbl.Api.Models.MemberNotInvitedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberNotInvitedToTeamExceptionHandler : IExceptionHandler<MemberNotInvitedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, MemberNotInvitedToTeamException exception)
        {
            var response = new MemberNotInvitedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}