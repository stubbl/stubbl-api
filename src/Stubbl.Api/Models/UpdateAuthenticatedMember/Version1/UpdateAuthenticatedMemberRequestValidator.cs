namespace Stubbl.Api.Models.UpdateAuthenticatedMember.Version1
{
   using FluentValidation;

   public class UpdateAuthenticatedMemberRequestValidator : AbstractValidator<UpdateAuthenticatedMemberRequest>
   {
      public UpdateAuthenticatedMemberRequestValidator()
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