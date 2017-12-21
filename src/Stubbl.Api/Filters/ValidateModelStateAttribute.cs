namespace Stubbl.Api.Filters
{
   using System.Linq;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.Filters;
   using Microsoft.Extensions.Logging;
   using Models.ValidationFailed.Version1;

   public class ValidateModelStateAttribute : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (context.ModelState.IsValid)
         {
            return;
         }

         var validationErrors = context.ModelState.Where(ms => ms.Value.Errors.Any(e => !string.IsNullOrWhiteSpace(e.ErrorMessage)))
            .Select(kvp => new ValidationError(kvp.Key, kvp.Value.Errors.Select(e => e.ErrorMessage).First()))
            .OrderBy(ve => ve.PropertyName)
            .ToList();

         var logger = (ILogger<ValidateModelStateAttribute>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ValidateModelStateAttribute>));
         logger.LogInformation
         (
            "ModelState is invalid. {@ValidationErrors}",
            validationErrors
         );

         var response = validationErrors.Any() ? new ValidationFailedResponse(validationErrors) : null;
         var result = new BadRequestObjectResult(response);

         context.Result = result;
      }
   }
}