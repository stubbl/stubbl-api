namespace Microsoft.AspNetCore.Mvc.ApplicationModels
{
   using System.Linq;
   using Filters;
   using ModelBinding;

   public class FromBodyRequiredConvention : IActionModelConvention
   {
      public void Apply(ActionModel action)
      {
         var parameterName = action.Parameters
            .Where(p => p.BindingInfo?.BindingSource.CanAcceptDataFrom(BindingSource.Body) ?? false)
            .Select(p => p.ParameterName)
            .SingleOrDefault();

         if (parameterName != null)
         {
            action.Filters.Add(new FromBodyRequiredActionFilter(parameterName));
         }
      }

      private class FromBodyRequiredActionFilter : IActionFilter
      {
         private readonly string _parameterName;

         public FromBodyRequiredActionFilter(string parameterName)
         {
            _parameterName = parameterName;
         }

         public void OnActionExecuting(ActionExecutingContext context)
         {
            context.ActionArguments.TryGetValue(_parameterName, out object value);

            if (value != null)
            {
               return;
            }

            context.Result = new StatusCodeResult(411);
         }

         public void OnActionExecuted(ActionExecutedContext context)
         {
         }
      }
   }
}