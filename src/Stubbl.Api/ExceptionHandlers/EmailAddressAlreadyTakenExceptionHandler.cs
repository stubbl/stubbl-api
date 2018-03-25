using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Stubbl.Api.Exceptions.EmailAdressAlreadyUsed.Version1;

namespace Stubbl.Api.ExceptionHandlers
{
    public class EmailAddressAlreadyTakenExceptionHandler : IExceptionHandler<EmailAddressAlreadyTakenException>
    {
        public async Task HandleAsync(HttpContext context, EmailAddressAlreadyTakenException exception)
        {
            var response = new EmailAddressAlreadyTakenResponse();

            await context.Response.WriteJsonAsync(HttpStatusCode.Conflict, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}