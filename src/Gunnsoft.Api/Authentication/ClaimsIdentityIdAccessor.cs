using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public class ClaimsIdentityIdAccessor : IIdentityIdAccessor
    {
        private readonly Lazy<string> _identityId;

        public ClaimsIdentityIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _identityId = new Lazy<string>(() =>
                httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "sub")?.Value);
        }

        public string IdentityId => _identityId.Value;
    }
}