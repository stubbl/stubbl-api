using System.Collections.Generic;
using Stubbl.Api.Models.Shared.Version1;

namespace Stubbl.Api.Models.CreateTeamStub.Version1
{
    public class CreateTeamStubRequest
    {
        public CreateTeamStubRequest()
        {
            Request = new Request();
            Response = new Response();
        }

        public string Name { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }
        public IReadOnlyCollection<string> Tags { get; set; }
    }
}