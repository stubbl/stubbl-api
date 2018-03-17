using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;
using Stubbl.Api.Models.RoleNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class RoleNotFoundExceptionHandler : IExceptionHandler<RoleNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, RoleNotFoundException exception)
        {
            var response = new RoleNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}