using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.FindTeamStub.Version1
{
    public class FindTeamStubProjection : IProjection
    {
        public FindTeamStubProjection(string id, string teamId, string name, Request request, Response response,
            IReadOnlyCollection<string> tags)
        {
            Id = id;
            TeamId = teamId;
            Name = name;
            Request = request;
            Response = response;
            Tags = tags;
        }

        public string Id { get; }
        public string Name { get; }
        public Request Request { get; }
        public Response Response { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public string TeamId { get; }
    }
}