namespace Stubbl.Api.Core.Events.TeamStubDeleted.Version1
{
   using Gunnsoft.Cqs.Events;
   using MongoDB.Bson;

   public class TeamStubDeletedEvent : IEvent
   {
      public TeamStubDeletedEvent(ObjectId teamId, ObjectId stubId)
      {
         TeamId = teamId;
         StubId = stubId;
      }

      public ObjectId StubId { get; }
      public ObjectId TeamId { get; }
   }
}
