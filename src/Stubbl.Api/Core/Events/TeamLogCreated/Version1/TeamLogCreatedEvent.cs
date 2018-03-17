﻿namespace Stubbl.Api.Core.Events.TeamLogCreated.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Events;
   using MongoDB.Bson;

   public class TeamLogCreatedEvent : IEvent
   {
      public TeamLogCreatedEvent(ObjectId teamId, ObjectId logId, IReadOnlyCollection<ObjectId> stubIds, Request request, Response response)
      {
         TeamId = teamId;
         LogId = LogId;
         StubIds = stubIds;
         Request = request;
         Response = response;
      }

      public ObjectId LogId { get; }
      public Request Request { get; }
      public Response Response { get; }
      public IReadOnlyCollection<ObjectId> StubIds { get; }
      public ObjectId TeamId { get; }
   }
}
