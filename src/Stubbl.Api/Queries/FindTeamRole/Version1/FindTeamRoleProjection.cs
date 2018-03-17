using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.FindTeamRole.Version1
{
    public class FindTeamRoleProjection : IProjection
    {
        public FindTeamRoleProjection(string id, string name, IReadOnlyCollection<Permission> permissions)
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