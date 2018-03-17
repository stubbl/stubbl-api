using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Models.MemberCannotManageTeams.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberCannotManageTeamsExceptionHandler : IExceptionHandler<MemberCannotManageTeamsException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotManageTeamsException exception)
        {
            var response = new MemberCannotManageTeamsResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}