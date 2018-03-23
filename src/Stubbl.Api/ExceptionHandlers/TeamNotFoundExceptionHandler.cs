using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.TeamNotFound.Version1;
using Stubbl.Api.Models.TeamNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class TeamNotFoundExceptionHandler : IExceptionHandler<TeamNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, TeamNotFoundException exception)
        {
            var response = new TeamNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}