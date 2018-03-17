using System.Collections.Generic;
using Gunnsoft.Api.Models.Error.Version1;

namespace Gunnsoft.Api.Models.ValidationFailed.Version1
{
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