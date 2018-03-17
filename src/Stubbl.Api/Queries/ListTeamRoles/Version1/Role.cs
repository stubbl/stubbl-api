using System.Collections.Generic;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListTeamRoles.Version1
{
    public class Role
    {
        public Role(string id, string name, IReadOnlyCollection<Permission> permissions)
        {
            Id = id;
            Name = name;
            Permissions = permissions;
        }

        public string Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
    }
}