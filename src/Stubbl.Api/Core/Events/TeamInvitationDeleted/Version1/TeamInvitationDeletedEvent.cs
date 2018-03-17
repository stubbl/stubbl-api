namespace Stubbl.Api.Core.Events.TeamInvitationDeleted.Version1
{
   using Gunnsoft.Cqs.Events;
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