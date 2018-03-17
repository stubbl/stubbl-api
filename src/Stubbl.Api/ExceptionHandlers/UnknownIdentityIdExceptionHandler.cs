using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Authentication;
using Stubbl.Api.Models.UnknownIdentityId.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class UnknownIdentityIdExceptionHandler : IExceptionHandler<UnknownIdentityIdException>
    {
        public async Task HandleAsync(HttpContext context, UnknownIdentityIdException exception)
        {
            var response = new UnknownIdentityIdResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Unauthorized, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}