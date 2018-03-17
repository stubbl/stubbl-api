using System.Collections.Generic;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Events.TeamRoleCreated.Version1;

namespace Stubbl.Api.Commands.CreateTeamRole.Version1
{
    public class CreateTeamRoleCommand : ICommand<TeamRoleCreatedEvent>
    {
        public CreateTeamRoleCommand(ObjectId teamId, string name,
            IReadOnlyCollection<Permission> permissions)
        {
            TeamId = teamId;
            Name = name;
            Permissions = permissions;
        }

        public string Name { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
        public ObjectId TeamId { get; }
    }
}