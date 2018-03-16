namespace Stubbl.Api.Core.Events.AuthenticatedUserInvitationDeclined.Version1
{
   using CodeContrib.Events;
   using MongoDB.Bson;

   public class AuthenticatedUserInvitationDeclinedEvent : IEvent
   {
      public AuthenticatedUserInvitationDeclinedEvent(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}
