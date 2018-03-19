using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Queries.CountPermissions.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountPermissionsQueryHandler : IQueryHandler<CountPermissionsQuery, CountPermissionsProjection>
    {
        public Task<CountPermissionsProjection> HandleAsync(CountPermissionsQuery query,
            CancellationToken cancellationToken)
        {
            var totalCount = Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Count();

            var projection = new CountPermissionsProjection
            (
                totalCount
            );

            return Task.FromResult(projection);
        }
    }
}