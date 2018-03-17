using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Models.InvitationAlreadyUsed.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class InvitationAlreadyUsedExceptionHandler : IExceptionHandler<InvitationAlreadyUsedException>
    {
        public async Task HandleAsync(HttpContext context, InvitationAlreadyUsedException exception)
        {
            var response = new InvitationAlreadyUsedResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}