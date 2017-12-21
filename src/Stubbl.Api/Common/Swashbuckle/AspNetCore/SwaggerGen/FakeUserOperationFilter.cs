namespace Swashbuckle.AspNetCore.SwaggerGen
{
   using Swashbuckle.AspNetCore.Swagger;
   using System.Collections.Generic;

   public class FakeUserOperationFilter : IOperationFilter
   {
      public void Apply(Operation operation, OperationFilterContext context)
      {
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
