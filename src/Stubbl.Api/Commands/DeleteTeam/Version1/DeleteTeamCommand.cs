using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamDeleted.Version1;

namespace Stubbl.Api.Commands.DeleteTeam.Version1
{
    public class DeleteTeamCommand : ICommand<TeamDeletedEvent>
    {
        public DeleteTeamCommand(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}