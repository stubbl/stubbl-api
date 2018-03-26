using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Models.AuthenticatedUserNotFound.Version1;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers.AuthenticatedUserNotFound.Version1
{
    public class AuthenticatedUserNotFoundExceptionHandler : IExceptionHandler<AuthenticatedUserNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, AuthenticatedUserNotFoundException exception)
        {
            var response = new AuthenticatedUserNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}