using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeams.Version1
{
    public class CountTeamsProjection : IProjection
    {
        public CountTeamsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}