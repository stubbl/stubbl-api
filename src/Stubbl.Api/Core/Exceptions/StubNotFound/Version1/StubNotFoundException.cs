namespace Stubbl.Api.Core.Exceptions.StubNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class StubNotFoundException : Exception
   {
      public StubNotFoundException(ObjectId stubId, ObjectId teamId)
         : base($"Stub not found. StubID='{stubId}' TeamID='{teamId}'")
      {
         StubId = stubId;
         TeamId = teamId;
      }

      public ObjectId StubId { get; }
      public ObjectId TeamId { get; }
   }
}