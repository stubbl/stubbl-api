namespace Stubbl.Api.Core.Events.TeamInvitationDeleted.Version1
{
   using Common.Events;
   using MongoDB.Bson;

   public class TeamInvitationDeletedEvent : IEvent
   {
      public TeamInvitationDeletedEvent(ObjectId teamId, ObjectId invitationId)
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}