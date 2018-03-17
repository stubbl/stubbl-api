using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeamLog.Version1
{
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