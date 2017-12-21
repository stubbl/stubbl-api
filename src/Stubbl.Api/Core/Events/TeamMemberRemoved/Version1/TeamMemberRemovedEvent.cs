namespace Stubbl.Api.Core.Events.TeamMemberRemoved.Version1
{
   using Common.Events;
   using MongoDB.Bson;

   public class TeamMemberRemovedEvent : IEvent
   {
      public TeamMemberRemovedEvent(ObjectId teamId, ObjectId memberId)
      {
         TeamId = teamId;
         MemberId = memberId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}
