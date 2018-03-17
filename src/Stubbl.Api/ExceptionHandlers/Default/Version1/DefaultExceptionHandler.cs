using System.Net;
using System.Threading.Tasks;
using Gunnsoft.Api.ExceptionHandlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stubbl.Api.Models.Error.Version1;
using Stubbl.Api.Models.Exception.Version1;
using Exception = System.Exception;

namespace Stubbl.Api.ExceptionHandlers.Default.Version1
{
    using SystemException = Exception;

    public class DefaultExceptionHandler : IDefaultExceptionHandler
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public DefaultExceptionHandler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task HandleAsync(HttpContext context, SystemException exception)
        {
            ErrorResponse response;

            if (_hostingEnvironment.IsDevelopment())
            {
                response = new ExceptionResponse
                (
                    new Models.Exception.Version1.Exception(exception)
                );
            }
            else
            {
                response = new ErrorResponse();
            }

            await context.Response.WriteJsonAsync(HttpStatusCode.InternalServerError, response,
                JsonConstants.JsonSerializerSettings);
        }
    }
}