namespace Stubbl.Api.Core.Queries.FindLog.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class FindTeamLogQuery : IQuery<FindTeamLogProjection>
   {
      public FindTeamLogQuery(ObjectId teamId, ObjectId logId)
      {
         TeamId = teamId;
         LogId = logId;
      }

      public ObjectId LogId { get; }
      public ObjectId TeamId { get; }
   }
}