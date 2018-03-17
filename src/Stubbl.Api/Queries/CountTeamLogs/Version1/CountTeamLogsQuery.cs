using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.CountTeamLogs.Version1
{
    public class CountTeamLogsQuery : IQuery<CountTeamLogsProjection>
    {
        public CountTeamLogsQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}