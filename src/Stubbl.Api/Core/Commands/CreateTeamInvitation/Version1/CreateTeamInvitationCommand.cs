namespace Stubbl.Api.Core.Commands.CreateTeamInvitation.Version1
{
   using CodeContrib.Commands;
   using Events.TeamInvitationCreated.Version1;
   using MongoDB.Bson;

   public class CreateTeamInvitationCommand : ICommand<TeamInvitationCreatedEvent>
   {
      public CreateTeamInvitationCommand(ObjectId teamId, ObjectId roleId, string emailAddress)
      {
         TeamId = teamId;
         RoleId = roleId;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}