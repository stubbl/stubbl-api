using Microsoft.AspNetCore.Builder;
using Stubbl.Api.Middleware.JsonExceptions;
using Stubbl.Api.Middleware.SecureRequests;
using Stubbl.Api.Middleware.Stub;
using Stubbl.Api.Middleware.SubHeader;

namespace Stubbl.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonExceptions(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<JsonExceptionsMiddleware>();

            return extended;
        }

        public static IApplicationBuilder UseFakeUser(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<SubHeaderMiddleware>();

            return extended;
        }

        public static IApplicationBuilder UseSecureRequests(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<SecureRequestsMiddleware>();

            return extended;
        }

        public static IApplicationBuilder UseStub(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<StubMiddleware>();

            return extended;
        }
    }
}