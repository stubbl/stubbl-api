using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.RoleAlreadyExists.Version1;
using Stubbl.Api.Models.RoleAlreadyExists.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class RoleAlreadyExistsExceptionHandler : IExceptionHandler<RoleAlreadyExistsException>
    {
        public async Task HandleAsync(HttpContext context, RoleAlreadyExistsException exception)
        {
            var response = new RoleAlreadyExistsResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}