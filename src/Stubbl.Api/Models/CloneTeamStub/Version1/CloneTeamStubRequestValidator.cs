namespace Stubbl.Api.Models.CloneTeamStub.Version1
{
   using FluentValidation;

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