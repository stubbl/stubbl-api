namespace Stubbl.Api.Core.Commands.ResendTeamInvitation.Version1
{
   using Common.Commands;
   using Events.TeamInvitationResent.Version1;
   using MongoDB.Bson;

   public class ResendTeamInvitationCommand : ICommand<TeamInvitationResentEvent>
   {
      public ResendTeamInvitationCommand(ObjectId teamId, ObjectId invitationId)
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}