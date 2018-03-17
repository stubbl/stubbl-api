namespace Stubbl.Api.Core.Queries.CountTeamLogs.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class CountTeamLogsQuery : IQuery<CountTeamLogsProjection>
   {
      public CountTeamLogsQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}