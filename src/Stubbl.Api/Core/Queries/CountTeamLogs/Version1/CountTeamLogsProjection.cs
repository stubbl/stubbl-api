namespace Stubbl.Api.Core.Queries.CountTeamLogs.Version1
{
   using Common.Queries;

   public class CountTeamLogsProjection : IProjection
   {
      public CountTeamLogsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}