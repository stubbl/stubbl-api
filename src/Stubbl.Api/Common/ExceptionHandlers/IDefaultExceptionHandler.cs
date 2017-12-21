namespace Stubbl.Api.Common.ExceptionHandlers
{
   using System;
   using System.Threading.Tasks;
   using Microsoft.AspNetCore.Http;

   public interface IDefaultExceptionHandler
   {
      Task HandleAsync(HttpContext context, Exception exception);
   }
}
