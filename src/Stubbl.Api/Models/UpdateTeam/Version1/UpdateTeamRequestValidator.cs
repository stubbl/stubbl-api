namespace Stubbl.Api.Models.UpdateTeam.Version1
{
   using FluentValidation;

   public class UpdateTeamRequestValidator : AbstractValidator<UpdateTeamRequest>
   {
      public UpdateTeamRequestValidator()
      {
         RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.")
            .Matches(@"^[\w\d]*$")
            .WithMessage(@"'{PropertyName}' must match '^[\w\d]*$'.");
      }
   }
}