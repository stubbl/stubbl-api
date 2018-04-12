using FluentValidation;

namespace Stubbl.Api.Models.CreateAuthenticatedUser.Version1
{
    public class CreateAuthenticatedUserRequestValidator : AbstractValidator<CreateAuthenticatedUserRequest>
    {
        public CreateAuthenticatedUserRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            RuleFor(m => m.EmailAddress)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.")
                .EmailAddress()
                .WithMessage("'{PropertyName}' must be a valid email address.");
        }
    }
}