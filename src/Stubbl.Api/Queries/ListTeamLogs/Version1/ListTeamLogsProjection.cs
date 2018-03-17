using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListTeamLogs.Version1
{
    public class ListTeamLogsProjection : IProjection
    {
        public ListTeamLogsProjection(IReadOnlyCollection<Log> logs, Paging paging)
        {
            Logs = logs;
            Paging = paging;
        }

        public IReadOnlyCollection<Log> Logs { get; }
        public Paging Paging { get; }
    }
}