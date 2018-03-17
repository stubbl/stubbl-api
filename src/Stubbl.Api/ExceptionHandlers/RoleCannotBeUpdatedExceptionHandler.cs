using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Exceptions.RoleCannotBeUpdated.Version1;
using Stubbl.Api.Models.RoleCannotBeUpdated.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class RoleCannotBeUpdatedExceptionHandler : IExceptionHandler<RoleCannotBeUpdatedException>
    {
        public async Task HandleAsync(HttpContext context, RoleCannotBeUpdatedException exception)
        {
            var response = new RoleCannotBeUpdatedResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}