using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListPermissions.Version1
{
    public class ListPermissionsProjection : IProjection
    {
        public ListPermissionsProjection(IReadOnlyCollection<Permission> permissions)
        {
            Permissions = permissions;
        }

        public IReadOnlyCollection<Permission> Permissions { get; }
    }
}