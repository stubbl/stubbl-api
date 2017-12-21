namespace Stubbl.Api.Middleware
{
   using JsonExceptions;
   using Microsoft.AspNetCore.Builder;
   using SecureRequests;
   using Stubbl.Api.Middleware.FakeUser;
   using StubTester;

   public static class MiddlewareExtensions
   {
      public static IApplicationBuilder UseJsonExceptions(this IApplicationBuilder applicationBuilder)
      {
         applicationBuilder.UseMiddleware<JsonExceptionsMiddleware>();

         return applicationBuilder;
      }

      public static IApplicationBuilder UseFakeUser(this IApplicationBuilder applicationBuilder)
      {
         applicationBuilder.UseMiddleware<FakeUserMiddleware>();

         return applicationBuilder;
      }

      public static IApplicationBuilder UseSecureRequests(this IApplicationBuilder applicationBuilder)
      {
         applicationBuilder.UseMiddleware<SecureRequestsMiddleware>();

         return applicationBuilder;
      }

      public static IApplicationBuilder UseStubTester(this IApplicationBuilder applicationBuilder)
      {
         applicationBuilder.UseMiddleware<StubTesterMiddleware>();

         return applicationBuilder;
      }
   }
}