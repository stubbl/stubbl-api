using FluentValidation;

namespace Stubbl.Api.Models.UpdateTeamStub.Version1
{
    public class UpdateTeamStubRequestValidator : AbstractValidator<UpdateTeamStubRequest>
    {
        public UpdateTeamStubRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            RuleFor(m => m.Request.HttpMethod)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            RuleFor(m => m.Request.Path)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            RuleFor(m => m.Response.HttpStatusCode)
                .NotNull()
                .WithMessage("'{PropertyName}' is required.");
        }
    }
}