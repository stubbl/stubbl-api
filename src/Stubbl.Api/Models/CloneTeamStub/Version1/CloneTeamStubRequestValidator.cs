using FluentValidation;

namespace Stubbl.Api.Models.CloneTeamStub.Version1
{
    public class CloneTeamStubRequestValidator : AbstractValidator<CloneTeamStubRequest>
    {
        public CloneTeamStubRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");
        }
    }
}