using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Models.MemberCannotManageInvitations.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class
        MemberCannotManageInvitationsExceptionHandler : IExceptionHandler<MemberCannotManageInvitationsException>
    {
        public async Task HandleAsync(HttpContext context, MemberCannotManageInvitationsException exception)
        {
            var response = new MemberCannotManageInvitationsResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Forbidden, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}