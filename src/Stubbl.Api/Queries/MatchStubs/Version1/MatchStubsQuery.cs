using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.MatchStubs.Version1
{
    public class MatchStubsQuery : IQuery<MatchStubsProjection>
    {
        public MatchStubsQuery(ObjectId teamId, Request request)
        {
            TeamId = teamId;
            Request = request;
        }

        public Request Request { get; }
        public ObjectId TeamId { get; }
    }
}