using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.StubNotFound.Version1;
using Stubbl.Api.Models.StubNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class StubNotFoundExceptionHandler : IExceptionHandler<StubNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, StubNotFoundException exception)
        {
            var response = new StubNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}