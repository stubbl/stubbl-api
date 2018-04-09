using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ControllerAttributes().OfType<AllowAnonymousAttribute>().Any()
                || context.ApiDescription.ActionAttributes().OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new Response {Description = "Unauthorized"});
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", new Response { Description = "Forbidden" });
            }

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", new[] {"stubbl-api"}}
                }
            };
        }
    }
}