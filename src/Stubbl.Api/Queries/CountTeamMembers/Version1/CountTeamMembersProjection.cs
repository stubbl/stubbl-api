using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeamMembers.Version1
{
    public class CountTeamMembersProjection : IProjection
    {
        public CountTeamMembersProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}