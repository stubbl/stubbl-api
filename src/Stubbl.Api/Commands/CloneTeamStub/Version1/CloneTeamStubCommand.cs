using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamStubCloned.Version1;

namespace Stubbl.Api.Commands.CloneTeamStub.Version1
{
    public class CloneTeamStubCommand : ICommand<TeamStubClonedEvent>
    {
        public CloneTeamStubCommand(ObjectId teamId, ObjectId stubId, string name)
        {
            TeamId = teamId;
            StubId = stubId;
            Name = name;
        }

        public ObjectId StubId { get; }
        public string Name { get; }
        public ObjectId TeamId { get; }
    }
}