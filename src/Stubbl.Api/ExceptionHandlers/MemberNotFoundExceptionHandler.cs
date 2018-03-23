using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.MemberNotFound.Version1;
using Stubbl.Api.Models.MemberNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberNotFoundExceptionHandler : IExceptionHandler<MemberNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, MemberNotFoundException exception)
        {
            var response = new MemberNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}