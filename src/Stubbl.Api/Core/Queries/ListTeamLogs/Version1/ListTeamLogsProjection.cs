namespace Stubbl.Api.Core.Queries.ListTeamLogs.Version1
{
   using System.Collections.Generic;
   using CodeContrib.Queries;
   using Shared.Version1;

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