using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.ListTeamLogs.Version1
{
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