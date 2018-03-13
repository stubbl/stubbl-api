namespace Stubbl.Api.Core.Commands.UpdateAuthenticatedUser.Version1
{
   using Common.Commands;
   using Events.AuthenticatedUserUpdated.Version1;

   public class UpdateAuthenticatedUserCommand : ICommand<AuthenticatedUserUpdatedEvent>
   {
      public UpdateAuthenticatedUserCommand(string name, string emailAddress)
      {
         Name = name;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public string Name { get; }
   }
}
