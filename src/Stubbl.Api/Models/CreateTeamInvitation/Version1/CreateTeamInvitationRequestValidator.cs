namespace Stubbl.Api.Models.CreateTeamInvitation.Version1
{
   using FluentValidation;
   using FluentValidation.Validators.ObjectId;

   public class CreateTeamInvitationRequestValidator : AbstractValidator<CreateTeamInvitationRequest>
   {
      public CreateTeamInvitationRequestValidator()
      {
         RuleFor(m => m.EmailAddress)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.")
            .EmailAddress()
            .WithMessage("'{PropertyName}' must be a valid email address.");

         RuleFor(m => m.RoleId)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.")
            .ObjectId()
            .WithMessage("'{PropertyName}' must be a valid ID.");
      }
   }
}