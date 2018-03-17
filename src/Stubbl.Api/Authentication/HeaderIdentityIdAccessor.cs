using System;
using Microsoft.AspNetCore.Http;

namespace Stubbl.Api.Authentication
{
    public class HeaderIdentityIdAccessor : IIdentityIdAccessor
    {
        private readonly Lazy<string> _httpContextAccessor;

        public HeaderIdentityIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = new Lazy<string>(() => httpContextAccessor.HttpContext.Request.Headers["X-Sub"]);
        }

        public string IdentityId => _httpContextAccessor.Value;
    }
}