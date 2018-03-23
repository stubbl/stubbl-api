using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Models.MemberCannotManageStubs.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberCannotManageStubsExceptionHandler : IExceptionHandler<MemberCannotManageStubsException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotManageStubsException exception)
        {
            var response = new MemberCannotManageStubsResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}