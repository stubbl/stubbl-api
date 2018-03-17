using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.ExceptionHandlers
{
    public interface IDefaultExceptionHandler
    {
        Task HandleAsync(HttpContext context, Exception exception);
    }
}