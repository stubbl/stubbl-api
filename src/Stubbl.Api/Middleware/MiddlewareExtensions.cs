namespace Stubbl.Api.Middleware
{
    using JsonExceptions;
    using Microsoft.AspNetCore.Builder;
    using SecureRequests;
    using SubHeader;
    using Stub;

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