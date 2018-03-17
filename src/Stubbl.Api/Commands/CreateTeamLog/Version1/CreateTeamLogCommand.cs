using System.Collections.Generic;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamLogCreated.Version1;

namespace Stubbl.Api.Commands.CreateTeamLog.Version1
{
    public class CreateTeamLogCommand : ICommand<TeamLogCreatedEvent>
    {
        public CreateTeamLogCommand(ObjectId teamId, IReadOnlyCollection<ObjectId> stubIds, Request request,
            Response response)
        {
            TeamId = teamId;
            StubIds = stubIds;
            Request = request;
            Response = response;
        }

        public Request Request { get; }
        public Response Response { get; }
        public IReadOnlyCollection<ObjectId> StubIds { get; }
        public ObjectId TeamId { get; }
    }
}