using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers
{
    public interface IExceptionHandler<in TException> where TException : Exception
    {
        Task HandleAsync(HttpContext context, TException exception);
    }
}