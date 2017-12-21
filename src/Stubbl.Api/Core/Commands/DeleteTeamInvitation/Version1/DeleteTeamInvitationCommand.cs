namespace Stubbl.Api.Core.Commands.DeleteTeamInvitation.Version1
{
   using Common.Commands;
   using Events.TeamInvitationDeleted.Version1;
   using MongoDB.Bson;

   public class DeleteTeamInvitationCommand : ICommand<TeamInvitationDeletedEvent>
   {
      public DeleteTeamInvitationCommand(ObjectId teamId, ObjectId invitationId)
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}