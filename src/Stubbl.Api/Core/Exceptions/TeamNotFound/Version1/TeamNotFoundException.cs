namespace Stubbl.Api.Core.Exceptions.TeamNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class TeamNotFoundException : Exception
   {
      public TeamNotFoundException(ObjectId teamId)
         : base($"Team cannot be found. TeamID='{teamId}'")
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}