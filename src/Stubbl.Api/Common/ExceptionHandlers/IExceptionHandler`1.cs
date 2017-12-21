namespace Stubbl.Api.Common.ExceptionHandlers
{
   using System;
   using System.Threading.Tasks;
   using Microsoft.AspNetCore.Http;

   public interface IExceptionHandler<in TException> where TException : Exception
   {
      Task HandleAsync(HttpContext context, TException exception);
   }
}
