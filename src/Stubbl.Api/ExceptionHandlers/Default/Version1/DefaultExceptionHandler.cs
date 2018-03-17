namespace Stubbl.Api.ExceptionHandlers.Default.Version1
{
   using System.Net;
   using System.Threading.Tasks;
   using Gunnsoft.Api.ExceptionHandlers;
   using Newtonsoft.Json;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.Http;
   using Models.Error.Version1;
   using Models.Exception.Version1;
   using SystemException = System.Exception;

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
               new Exception(exception)
            );
         }
         else
         {
            response = new ErrorResponse();
         }

         await context.Response.WriteJsonAsync(HttpStatusCode.InternalServerError, response, JsonConstants.JsonSerializerSettings);
      }
   }
}
