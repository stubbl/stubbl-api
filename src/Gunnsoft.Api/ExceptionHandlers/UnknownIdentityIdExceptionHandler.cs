using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.Exceptions.UnknownIdentityId.Version1;
using Gunnsoft.Api.Models.UnknownIdentityId.Version1;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers
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