using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public class ClaimsSubAccessor : ISubAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _sub;

        public ClaimsSubAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Sub
        {
            get
            {
                if (_sub != null)
                {
                    return _sub;
                }

                _sub = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "sub")?.Value;

                return _sub;
            }
        }
    }
}