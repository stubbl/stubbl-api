using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberCannotManageRoles.Version1;
using Stubbl.Api.Models.MemberCannotManageRoles.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class MemberCannotManageRolesExceptionHandler : IExceptionHandler<MemberCannotManageRolesException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotManageRolesException exception)
        {
            var response = new MemberCannotManageRolesResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}