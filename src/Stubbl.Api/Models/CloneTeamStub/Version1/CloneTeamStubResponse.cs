namespace Stubbl.Api.Models.CloneTeamStub.Version1
{
    public class CloneTeamStubResponse
    {
        public CloneTeamStubResponse(string stubId)
        {
            StubId = stubId;
        }

        public string StubId { get; }
    }
}