namespace Stubbl.Api.Core.Events.TeamInvitationResent.Version1
{
   using CodeContrib.Events;
   using MongoDB.Bson;

   public class TeamInvitationResentEvent : IEvent
   {
      public TeamInvitationResentEvent(ObjectId teamId, ObjectId invitationId)
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}