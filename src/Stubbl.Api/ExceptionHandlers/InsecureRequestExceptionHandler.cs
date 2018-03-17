using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Middleware.SecureRequests;
using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class InsecureRequestExceptionHandler : IExceptionHandler<InsecureRequestException>
    {
        public async Task HandleAsync(HttpContext context, InsecureRequestException exception)
        {
            var response = new ErrorResponse("InsecureRequest", "Requests must be made using HTTPS.");

            await context.Response.WriteJsonAsync(HttpStatusCode.HttpVersionNotSupported, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}