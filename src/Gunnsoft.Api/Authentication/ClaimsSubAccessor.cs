using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public class ClaimsSubAccessor : ISubAccessor
    {
        private readonly Lazy<string> _sub;

        public ClaimsSubAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _sub = new Lazy<string>(() =>
                httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "sub")?.Value);
        }

        public string Sub => _sub.Value;
    }
}