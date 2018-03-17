using System.Collections.Generic;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Events.TeamStubUpdated.Version1;

namespace Stubbl.Api.Commands.UpdateTeamStub.Version1
{
    public class UpdateTeamStubCommand : ICommand<TeamStubUpdatedEvent>
    {
        public UpdateTeamStubCommand(ObjectId teamId, ObjectId stubId, string name, Request request,
            Response response, IReadOnlyCollection<string> tags)
        {
            TeamId = teamId;
            StubId = stubId;
            Name = name;
            Request = request;
            Response = response;
            Tags = tags;
        }

        public string Name { get; }
        public Request Request { get; }
        public Response Response { get; }
        public ObjectId StubId { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public ObjectId TeamId { get; }
    }
}