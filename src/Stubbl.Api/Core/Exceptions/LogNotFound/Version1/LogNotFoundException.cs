namespace Stubbl.Api.Core.Exceptions.LogNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class LogNotFoundException : Exception
   {
      public LogNotFoundException(ObjectId logId, ObjectId teamId)
         : base($"Log not found. LogID='{logId}' TeamID='{teamId}'")
      {
         LogId = logId;
         TeamId = teamId;
      }

      public ObjectId LogId { get; }
      public ObjectId TeamId { get; }
   }
}