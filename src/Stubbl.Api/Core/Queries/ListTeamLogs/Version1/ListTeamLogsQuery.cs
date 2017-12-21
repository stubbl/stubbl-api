namespace Stubbl.Api.Core.Queries.ListTeamLogs.Version1
{
   using System.Collections.Generic;
   using Common.Queries;
   using MongoDB.Bson;

   public class ListTeamLogsQuery : IQuery<ListTeamLogsProjection>
   {
      public ListTeamLogsQuery(ObjectId teamId, IReadOnlyCollection<ObjectId> stubIds, int pageNumber, int pageSize)
      {
         TeamId = teamId;
         StubIds = stubIds;
         PageNumber = pageNumber;
         PageSize = pageSize;
      }

      public int PageNumber { get; }
      public int PageSize { get; }
      public IReadOnlyCollection<ObjectId> StubIds { get; }
      public ObjectId TeamId { get; }
   }
}