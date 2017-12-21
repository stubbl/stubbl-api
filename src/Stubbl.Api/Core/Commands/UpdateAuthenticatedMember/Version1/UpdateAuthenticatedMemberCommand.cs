namespace Stubbl.Api.Core.Commands.UpdateAuthenticatedMember.Version1
{
   using Common.Commands;
   using Events.AuthenticatedMemberUpdated.Version1;

   public class UpdateAuthenticatedMemberCommand : ICommand<AuthenticatedMemberUpdatedEvent>
   {
      public UpdateAuthenticatedMemberCommand(string name, string emailAddress)
      {
         Name = name;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public string Name { get; }
   }
}
