using System.Collections.Generic;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Events.TeamStubCreated.Version1;

namespace Stubbl.Api.Commands.CreateTeamStub.Version1
{
    public class CreateTeamStubCommand : ICommand<TeamStubCreatedEvent>
    {
        public CreateTeamStubCommand(ObjectId teamId, string name, Request request,
            Response response, IReadOnlyCollection<string> tags)
        {
            TeamId = teamId;
            Name = name;
            Request = request;
            Response = response;
            Tags = tags;
        }

        public string Name { get; }
        public Request Request { get; }
        public Response Response { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public ObjectId TeamId { get; }
    }
}