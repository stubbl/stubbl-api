﻿namespace Stubbl.Api.Middleware.SecureRequests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    // TODO Move to Common
    public class SecureRequestsMiddleware
    {
        private readonly RequestDelegate _next;

        public SecureRequestsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.IsHttps)
            {
                throw new InsecureRequestException();
            }

            await _next(context);
        }
    }
}
