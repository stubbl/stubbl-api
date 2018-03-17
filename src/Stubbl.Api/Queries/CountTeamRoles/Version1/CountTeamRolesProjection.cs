using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeamRoles.Version1
{
    public class CountTeamRolesProjection : IProjection
    {
        public CountTeamRolesProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}