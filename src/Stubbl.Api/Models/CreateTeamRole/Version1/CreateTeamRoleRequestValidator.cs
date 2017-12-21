﻿namespace Stubbl.Api.Models.CreateTeamRole.Version1
{
   using FluentValidation;
   using Shared.Version1;

   public class CreateTeamRoleRequestValidator : AbstractValidator<CreateTeamRoleRequest>
   {
      public CreateTeamRoleRequestValidator()
      {
         RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleFor(m => m.Permissions)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleForEach(m => m.Permissions)
            .SetValidator(new    PermissionValidator());
      }
   }
}