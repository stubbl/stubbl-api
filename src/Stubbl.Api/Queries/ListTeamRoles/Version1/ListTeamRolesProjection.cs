using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.ListTeamRoles.Version1
{
    public class ListTeamRolesProjection : IProjection
    {
        public ListTeamRolesProjection(IReadOnlyCollection<Role> roles)
        {
            Roles = roles;
        }

        public IReadOnlyCollection<Role> Roles { get; }
    }
}