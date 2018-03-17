namespace Stubbl.Api.Core.Commands.AcceptAuthenticatedUserInvitation.Version1
{
   using Gunnsoft.Cqs.Commands;
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