using Microsoft.AspNetCore.Builder;
using Stubbl.Api.Middleware.StubMatcher;

namespace Stubbl.Api.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseStubMatcher(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<StubMatcherMiddleware>();

            return extended;
        }
    }
}