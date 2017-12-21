namespace Stubbl.Api.Models.CreateTeamStub.Version1
{
   using FluentValidation;

   public class CreateTeamStubRequestValidator : AbstractValidator<CreateTeamStubRequest>
   {
      public CreateTeamStubRequestValidator()
      {
         RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleFor(m => m.Request.HttpMethod)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleFor(m => m.Request.Path)
            .NotEmpty()
            .WithMessage("'{PropertyName}' is required.");

         RuleFor(m => m.Response.HttpStatusCode)
            .NotNull()
            .WithMessage("'{PropertyName}' is required.");
      }
   }
}