using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamUpdated.Version1;

namespace Stubbl.Api.Commands.UpdateTeam.Version1
{
    public class UpdateTeamCommand : ICommand<TeamUpdatedEvent>
    {
        public UpdateTeamCommand(ObjectId teamId, string name)
        {
            TeamId = teamId;
            Name = name;
        }

        public string Name { get; }
        public ObjectId TeamId { get; }
    }
}