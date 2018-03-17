using FluentValidation;

namespace Stubbl.Api.Models.UpdateAuthenticatedUser.Version1
{
    public class UpdateAuthenticatedUserRequestValidator : AbstractValidator<UpdateAuthenticatedUserRequest>
    {
        public UpdateAuthenticatedUserRequestValidator()
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