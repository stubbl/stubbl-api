using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.LogNotFound.Version1;
using Stubbl.Api.Models.LogNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class LogNotFoundExceptionHandler : IExceptionHandler<LogNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, LogNotFoundException exception)
        {
            var response = new LogNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}