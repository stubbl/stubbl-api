namespace Stubbl.Api.Core.Events.AuthenticatedMemberInvitationDeclined.Version1
{
   using Common.Events;
   using MongoDB.Bson;

   public class AuthenticatedMemberInvitationDeclinedEvent : IEvent
   {
      public AuthenticatedMemberInvitationDeclinedEvent(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}
