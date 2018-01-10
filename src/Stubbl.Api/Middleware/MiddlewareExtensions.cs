namespace Stubbl.Api.Middleware
{
   using JsonExceptions;
   using Microsoft.AspNetCore.Builder;
   using SecureRequests;
   using Stubbl.Api.Middleware.FakeUser;
   using StubTester;

   public static class MiddlewareExtensions
   {
      public static IApplicationBuilder UseJsonExceptions(this IApplicationBuilder app)
      {
         app.UseMiddleware<JsonExceptionsMiddleware>();

         return app;
      }

      public static IApplicationBuilder UseFakeUser(this IApplicationBuilder app)
      {
         app.UseMiddleware<FakeUserMiddleware>();

         return app;
      }

      public static IApplicationBuilder UseSecureRequests(this IApplicationBuilder app)
      {
         app.UseMiddleware<SecureRequestsMiddleware>();

         return app;
      }

      public static IApplicationBuilder UseStubTester(this IApplicationBuilder app)
      {
         app.UseMiddleware<StubTesterMiddleware>();

         return app;
      }
   }
}