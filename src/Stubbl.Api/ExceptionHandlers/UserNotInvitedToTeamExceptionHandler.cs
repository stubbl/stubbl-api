using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.UserNotInvitedToTeam.Version1;
using Stubbl.Api.Models.MemberNotInvitedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class UserNotInvitedToTeamExceptionHandler : IExceptionHandler<UserNotInvitedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, UserNotInvitedToTeamException exception)
        {
            var response = new MemberNotInvitedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}