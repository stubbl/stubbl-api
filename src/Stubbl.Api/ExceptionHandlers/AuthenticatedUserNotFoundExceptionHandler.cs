using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Stubbl.Api.Models.AuthenticatedUserNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class AuthenticatedUserNotFoundExceptionHandler : IExceptionHandler<AuthenticatedUserNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, AuthenticatedUserNotFoundException exception)
        {
            var response = new AuthenticatedUserNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Unauthorized, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}