namespace Swashbuckle.AspNetCore.SwaggerGen
{
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.AspNetCore.Swagger;
    using System.Collections.Generic;
    using System.Linq;

    public class SubHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ControllerAttributes().SingleOrDefault(a => a is AllowAnonymousAttribute) != null
                || context.ApiDescription.ActionDescriptor.FilterDescriptors.SingleOrDefault(a => a.Filter is AllowAnonymousAttribute) != null)
            {
                return;
            }

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "X-Sub",
                In = "header",
                Type = "string",
                Required = true
            });
        }
    }
}
