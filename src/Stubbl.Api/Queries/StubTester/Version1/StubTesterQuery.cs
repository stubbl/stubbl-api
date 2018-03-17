using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.StubTester.Version1
{
    public class StubTesterQuery : IQuery<StubTesterProjection>
    {
        public StubTesterQuery(ObjectId teamId, Request request)
        {
            TeamId = teamId;
            Request = request;
        }

        public Request Request { get; }
        public ObjectId TeamId { get; }
    }
}