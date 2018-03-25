using System;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public class HeaderSubAccessor : ISubAccessor
    {
        private readonly Lazy<string> _sub;

        public HeaderSubAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _sub = new Lazy<string>(() => httpContextAccessor.HttpContext.Request.Headers["X-Sub"]);
        }

        public string Sub => _sub.Value;
    }
}