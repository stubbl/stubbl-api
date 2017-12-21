namespace Stubbl.Api.Core.Commands.AcceptAuthenticatedMemberInvitation.Version1
{
   using Common.Commands;
   using Events.AuthenticatedMemberInvitationAccepted.Version1;
   using MongoDB.Bson;

   public class AcceptAuthenticatedMemberInvitationCommand : ICommand<AuthenticatedMemberInvitationAcceptedEvent>
   {
      public AcceptAuthenticatedMemberInvitationCommand(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}