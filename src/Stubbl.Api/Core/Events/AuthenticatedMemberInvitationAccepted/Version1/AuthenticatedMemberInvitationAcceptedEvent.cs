namespace Stubbl.Api.Core.Events.AuthenticatedMemberInvitationAccepted.Version1
{
   using Common.Events;
   using MongoDB.Bson;

   public class AuthenticatedMemberInvitationAcceptedEvent : IEvent
   {
      public AuthenticatedMemberInvitationAcceptedEvent(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}
