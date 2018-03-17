namespace Swashbuckle.AspNetCore.SwaggerGen
{
   using Swashbuckle.AspNetCore.Swagger;
   using System.Linq;

   public class SwaggerDefaultValuesOperationFilter : IOperationFilter
   {
      public void Apply(Operation operation, OperationFilterContext context)
      {
         if (operation.Parameters == null)
         {
            return;
         }

         foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
         {
            var parameterDescription = context.ApiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);

            if (parameterDescription == null)
            {
               continue;
            }

            if (parameterDescription.ModelMetadata != null)
            {
               if (parameter.Description == null)
               {
                  parameter.Description = parameterDescription?.ModelMetadata?.Description;
               }
            }

            if (parameterDescription.RouteInfo != null)
            {
               if (parameter.Default == null)
               {
                  parameter.Default = parameterDescription?.RouteInfo?.DefaultValue;
               }

               parameter.Required |= !parameterDescription.RouteInfo.IsOptional;
            }
         }
      }
   }
}
