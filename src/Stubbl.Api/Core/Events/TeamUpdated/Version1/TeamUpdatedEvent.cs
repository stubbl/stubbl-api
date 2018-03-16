namespace Stubbl.Api.Core.Events.TeamUpdated.Version1
{
   using CodeContrib.Events;
   using MongoDB.Bson;

   public class TeamUpdatedEvent : IEvent
   {
      public TeamUpdatedEvent(ObjectId teamId, string name)
      {
         TeamId = teamId;
         Name = name;
      }

      public string Name { get; }
      public ObjectId TeamId { get; }
   }
}
