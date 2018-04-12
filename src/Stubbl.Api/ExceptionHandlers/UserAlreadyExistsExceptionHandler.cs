using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.UserAlreadyExists.Version1;
using Stubbl.Api.Models.UserAlreadyExists.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class UserAlreadyExistsExceptionHandler : IExceptionHandler<UserAlreadyExistsException>
    {
        public async Task HandleAsync(HttpContext context, UserAlreadyExistsException exception)
        {
            var response = new UserAlreadyExistsResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}