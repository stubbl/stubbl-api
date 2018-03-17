using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeam.Version1
{
    public class FindTeamQuery : IQuery<FindTeamProjection>
    {
        public FindTeamQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}