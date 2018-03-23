using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Models.MemberNotAddedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class UserNotAddedToTeamExceptionHandler : IExceptionHandler<UserNotAddedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, UserNotAddedToTeamException exception)
        {
            var response = new MemberNotAddedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}