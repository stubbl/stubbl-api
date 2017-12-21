namespace Stubbl.Api.Core.Commands.DeclineAuthenticatedMemberInvitation.Version1
{
   using Common.Commands;
   using Events.AuthenticatedMemberInvitationDeclined.Version1;
   using MongoDB.Bson;

   public class DeclineAuthenticatedMemberInvitationCommand : ICommand<AuthenticatedMemberInvitationDeclinedEvent>
   {
      public DeclineAuthenticatedMemberInvitationCommand(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}