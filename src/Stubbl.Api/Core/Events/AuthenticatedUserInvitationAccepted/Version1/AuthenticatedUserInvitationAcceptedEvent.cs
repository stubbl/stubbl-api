namespace Stubbl.Api.Core.Events.AuthenticatedUserInvitationAccepted.Version1
{
   using CodeContrib.Events;
   using MongoDB.Bson;

   public class AuthenticatedUserInvitationAcceptedEvent : IEvent
   {
      public AuthenticatedUserInvitationAcceptedEvent(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}
