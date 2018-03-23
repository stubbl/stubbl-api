using Microsoft.AspNetCore.Http;

namespace Gunnsoft.Api.Authentication
{
    public class HeaderSubAccessor : ISubAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _sub;

        public HeaderSubAccessor(IHttpContextAccessor httpContextAccessor)
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

                _sub = _httpContextAccessor.HttpContext.Request.Headers["X-Sub"];

                return _sub;
            }
        }
    }
}