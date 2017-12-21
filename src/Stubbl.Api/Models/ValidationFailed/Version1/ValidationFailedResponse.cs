namespace Stubbl.Api.Models.ValidationFailed.Version1
{
   using System.Collections.Generic;
   using Error.Version1;

   public class ValidationFailedResponse : ErrorResponse
   {
      public ValidationFailedResponse(IReadOnlyCollection<ValidationError> validationErrors)
         : base("ValidationFailed", "The request contains one or more validation errors.")
      {
         ValidationErrors = validationErrors;
      }

      public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
   }
}