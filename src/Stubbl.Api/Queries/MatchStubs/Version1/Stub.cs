using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.MatchStubs.Version1
{
    public class Stub
    {
        public Stub(string stubId, Shared.Version1.Request request, Response response)
        {
            StubId = stubId;
            Request = request;
            Response = response;
        }

        public Shared.Version1.Request Request { get; }
        public Response Response { get; }
        public string StubId { get; }
    }
}