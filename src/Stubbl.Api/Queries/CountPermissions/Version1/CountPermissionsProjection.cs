using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountPermissions.Version1
{
    public class CountPermissionsProjection : IProjection
    {
        public CountPermissionsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}