using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeamLogs.Version1
{
    public class CountTeamLogsProjection : IProjection
    {
        public CountTeamLogsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}