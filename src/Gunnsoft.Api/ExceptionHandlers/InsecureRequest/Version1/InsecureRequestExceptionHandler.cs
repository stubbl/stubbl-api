using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.Middleware.SecureRequests;
using Gunnsoft.Api.Models.Error.Version1;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers.InsecureRequest.Version1
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