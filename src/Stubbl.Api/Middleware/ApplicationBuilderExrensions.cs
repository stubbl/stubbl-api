using Microsoft.AspNetCore.Builder;
using Stubbl.Api.Middleware.Stub;

namespace Stubbl.Api.Middleware
{
    public static class ApplicationBuilderExrensions
    {
        public static IApplicationBuilder UseStub(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<StubMiddleware>();

            return extended;
        }
    }
}