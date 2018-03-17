using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberCannotManageMembers.Version1;
using Stubbl.Api.Models.MemberCannotManageMembers.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberCannotManageMembersExceptionHandler : IExceptionHandler<MemberCannotManageMembersException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotManageMembersException exception)
        {
            var response = new MemberCannotManageMembersResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}