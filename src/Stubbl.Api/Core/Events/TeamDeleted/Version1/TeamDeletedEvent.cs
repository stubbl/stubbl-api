namespace Stubbl.Api.Core.Events.TeamDeleted.Version1
{
   using CodeContrib.Events;
   using MongoDB.Bson;

   public class TeamDeletedEvent : IEvent
   {
      public TeamDeletedEvent(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}
