using FluentValidation;

namespace Stubbl.Api.Models.CreateTeam.Version1
{
    public class CreateTeamRequestValidator : AbstractValidator<CreateTeamRequest>
    {
        public CreateTeamRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.")
                .Matches(@"^[\w\d-]*$")
                .WithMessage(@"'{PropertyName}' must match '^[\w\d-]*$'.");
        }
    }
}