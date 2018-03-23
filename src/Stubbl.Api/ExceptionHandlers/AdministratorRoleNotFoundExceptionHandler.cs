using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.AdministratorRoleNotFound.Version1;
using Stubbl.Api.Models.AdministratorRoleNotFound.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class AdministratorRoleNotFoundExceptionHandler : IExceptionHandler<AdministratorRoleNotFoundException>
    {
        public async Task HandleAsync(HttpContext context, AdministratorRoleNotFoundException exception)
        {
            var response = new AdministratorRoleNotFoundResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}