using System.Collections.Generic;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.Commands.UpdateTeamRole.Version1
{
    public class UpdateTeamRoleCommand : ICommand<TeamRoleUpdatedEvent>
    {
        public UpdateTeamRoleCommand(ObjectId teamId, ObjectId roleId, string name,
            IReadOnlyCollection<Permission> permissions)
        {
            TeamId = teamId;
            RoleId = roleId;
            Name = name;
            Permissions = permissions;
        }

        public string Name { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}