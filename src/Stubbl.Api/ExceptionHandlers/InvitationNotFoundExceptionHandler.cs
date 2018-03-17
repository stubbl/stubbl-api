using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.InvitationNotFound.Version1;
using Stubbl.Api.Models.InvitationNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class InvitationNotFoundExceptionHandler : IExceptionHandler<InvitationNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, InvitationNotFoundException exception)
        {
            var response = new InvitationNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}