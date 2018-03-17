using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.CountTeamStubs.Version1
{
    public class CountTeamStubsQuery : IQuery<CountTeamStubsProjection>
    {
        public CountTeamStubsQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}