using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamStubDeleted.Version1;

namespace Stubbl.Api.Commands.DeleteTeamStub.Version1
{
    public class DeleteTeamStubCommand : ICommand<TeamStubDeletedEvent>
    {
        public DeleteTeamStubCommand(ObjectId teamId, ObjectId stubId)
        {
            TeamId = teamId;
            StubId = stubId;
        }

        public ObjectId StubId { get; }
        public ObjectId TeamId { get; }
    }
}