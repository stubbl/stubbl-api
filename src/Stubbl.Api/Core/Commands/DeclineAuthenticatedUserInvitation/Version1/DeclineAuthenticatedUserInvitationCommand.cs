namespace Stubbl.Api.Core.Commands.DeclineAuthenticatedUserInvitation.Version1
{
   using CodeContrib.Commands;
   using Events.AuthenticatedUserInvitationDeclined.Version1;
   using MongoDB.Bson;

   public class DeclineAuthenticatedUserInvitationCommand : ICommand<AuthenticatedUserInvitationDeclinedEvent>
   {
      public DeclineAuthenticatedUserInvitationCommand(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}