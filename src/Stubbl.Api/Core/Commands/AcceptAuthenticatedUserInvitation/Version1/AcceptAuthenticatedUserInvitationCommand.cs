namespace Stubbl.Api.Core.Commands.AcceptAuthenticatedUserInvitation.Version1
{
   using Common.Commands;
   using Events.AuthenticatedUserInvitationAccepted.Version1;
   using MongoDB.Bson;

   public class AcceptAuthenticatedUserInvitationCommand : ICommand<AuthenticatedUserInvitationAcceptedEvent>
   {
      public AcceptAuthenticatedUserInvitationCommand(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}