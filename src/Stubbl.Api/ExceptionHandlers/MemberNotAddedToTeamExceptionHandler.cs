using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Models.MemberNotAddedToTeam.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberNotAddedToTeamExceptionHandler : IExceptionHandler<MemberNotAddedToTeamException>
    {
        public async Task HandleAsync(HttpContext context, MemberNotAddedToTeamException exception)
        {
            var response = new MemberNotAddedToTeamResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}