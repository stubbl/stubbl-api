using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.ListPermissions.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Permission = Stubbl.Api.Data.Collections.Shared.Permission;

namespace Stubbl.Api.QueryHandlers
{
    public class ListPermissionsQueryHandler : IQueryHandler<ListPermissionsQuery, ListPermissionsProjection>
    {
        public Task<ListPermissionsProjection> HandleAsync(ListPermissionsQuery query,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new ListPermissionsProjection
            (
                Enum.GetValues(typeof(Permission))
                    .Cast<Permission>()
                    .Where(p => p > 0)
                    .Select(p => p.ToQueryPermission())
                    .Where(p => p != null)
                    .ToList()
            ));
        }
    }
}