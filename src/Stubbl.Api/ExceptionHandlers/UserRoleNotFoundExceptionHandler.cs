using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.UserRoleNotFound.Version1;
using Stubbl.Api.Models.UserRoleNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class UserRoleNotFoundExceptionHandler : IExceptionHandler<UserRoleNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, UserRoleNotFoundException exception)
        {
            var response = new UserRoleNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}