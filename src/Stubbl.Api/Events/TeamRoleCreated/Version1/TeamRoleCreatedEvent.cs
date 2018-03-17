using System.Collections.Generic;
using Gunnsoft.Cqs.Events;
using MongoDB.Bson;
using Stubbl.Api.Events.Shared.Version1;

namespace Stubbl.Api.Events.TeamRoleCreated.Version1
{
    public class TeamRoleCreatedEvent : IEvent
    {
        public TeamRoleCreatedEvent(ObjectId roleId, ObjectId teamId, string name,
            IReadOnlyCollection<Permission> permissions)
        {
            RoleId = roleId;
            TeamId = teamId;
            Name = name;
            Permissions = permissions;
        }

        public string Name { get; }
        public ObjectId RoleId { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
        public ObjectId TeamId { get; }
    }
}