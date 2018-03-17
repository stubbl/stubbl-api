using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberCannotBeRemovedFromTeam.Version1;
using Stubbl.Api.Models.MemberCannotBeRemovedFromTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class
        MemberCannotBeRemovedFromTeamExceptionHandler : IExceptionHandler<MemberCannotBeRemovedFromTeamException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotBeRemovedFromTeamException exception)
        {
            var response = new MemberCannotBeRemovedFromTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}