using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamRoleDeleted.Version1;

namespace Stubbl.Api.Commands.DeleteTeamRole.Version1
{
    public class DeleteTeamRoleCommand : ICommand<TeamRoleDeletedEvent>
    {
        public DeleteTeamRoleCommand(ObjectId teamId, ObjectId roleId)
        {
            TeamId = teamId;
            RoleId = roleId;
        }

        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}