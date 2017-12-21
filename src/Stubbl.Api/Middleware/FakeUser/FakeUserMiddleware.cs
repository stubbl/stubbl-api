
namespace Stubbl.Api.Middleware.FakeUser
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Microsoft.AspNetCore.Http;
   using System.Security.Claims;

   public class FakeUserMiddleware
   {
      private readonly RequestDelegate _next;

      public FakeUserMiddleware(RequestDelegate next)
      {
         _next = next;
      }

      public async Task Invoke(HttpContext context)
      {
         context.Request.Headers.TryGetValue("X-Sub", out var headerSub);
         var sub = headerSub.FirstOrDefault();

         if (string.IsNullOrEmpty(sub))
         {
            await _next(context);

            return;
         }

         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "fakeuser@stubbl.it"),
            new Claim(ClaimTypes.Name, "Fake User"),
            new Claim("sub", sub),
         };
         var claimsIdentity = new ClaimsIdentity(claims, "FakeUser");

         var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
         context.User = claimsPrincipal;

         await _next(context);
      }
   }
}
