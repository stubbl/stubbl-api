using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.Exceptions.UnknownSub.Version1;
using Gunnsoft.Api.Models.UnknownSub.Version1;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers.UnknownSub.Version1
{
    public class UnknownSubExceptionHandler : IExceptionHandler<UnknownSubException>
    {
        public async Task HandleAsync(HttpContext context, UnknownSubException exception)
        {
            var response = new UnknownSubResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}