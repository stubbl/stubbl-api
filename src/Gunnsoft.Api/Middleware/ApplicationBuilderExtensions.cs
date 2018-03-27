using Gunnsoft.Api.Middleware.JsonExceptions;
using Gunnsoft.Api.Middleware.SecureRequests;
using Gunnsoft.Api.Middleware.SubHeader;
using Microsoft.AspNetCore.Builder;

namespace Gunnsoft.Api.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonExceptions(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<JsonExceptionsMiddleware>();

            return extended;
        }

        public static IApplicationBuilder UseSecureRequests(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<SecureRequestsMiddleware>();

            return extended;
        }

        public static IApplicationBuilder UseSubHeader(this IApplicationBuilder extended)
        {
            extended.UseMiddleware<SubHeaderMiddleware>();

            return extended;
        }
    }
}