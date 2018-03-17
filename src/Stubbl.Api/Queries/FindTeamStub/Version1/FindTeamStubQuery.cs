using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeamStub.Version1
{
    public class FindTeamStubQuery : IQuery<FindTeamStubProjection>
    {
        public FindTeamStubQuery(ObjectId teamId, ObjectId stubId)
        {
            TeamId = teamId;
            StubId = stubId;
        }

        public ObjectId TeamId { get; }
        public ObjectId StubId { get; }
    }
}