using System.Collections.Generic;
using MongoDB.Bson;
using Stubbl.Api.Events.Shared.Version1;

namespace Stubbl.Api.Events.TeamCreated.Version1
{
    public class Role
    {
        public Role(ObjectId id, string name, IReadOnlyCollection<Permission> permissions)
        {
            Id = id;
            Name = name;
            Permissions = permissions;
        }

        public ObjectId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
    }
}