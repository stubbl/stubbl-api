namespace Stubbl.Api.Core.Events.TeamStubUpdated.Version1
{
   using System.Collections.Generic;
   using CodeContrib.Events;
   using MongoDB.Bson;
   using Shared.Version1;

   public class TeamStubUpdatedEvent : IEvent
   {
      public TeamStubUpdatedEvent(ObjectId stubId, ObjectId teamId, string name, Request request, Response response, IReadOnlyCollection<string> tags)
      {
         StubId = stubId;
         TeamId = teamId;
         Name = name;
         Request = request;
         Response = response;
         Tags = tags;
      }

      public string Name { get; }
      public Request Request { get; }
      public Response Response { get; }
      public ObjectId StubId { get; }
      public IReadOnlyCollection<string> Tags { get; }
      public ObjectId TeamId { get; }
   }
}
