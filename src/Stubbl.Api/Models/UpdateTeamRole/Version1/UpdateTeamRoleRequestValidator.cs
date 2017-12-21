namespace Stubbl.Api.Models.UpdateTeamRole.Version1
{
   using FluentValidation;
   using Shared.Version1;

   public class UpdateTeamRoleRequestValidator : AbstractValidator<UpdateTeamRoleRequest>
   {
      public UpdateTeamRoleRequestValidator()
      {
         RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleFor(m => m.Permissions)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleForEach(m => m.Permissions)
            .SetValidator(new PermissionValidator());
      }
   }
}