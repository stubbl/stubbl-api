namespace Stubbl.Api.Core.Queries.CountTeams.Version1
{
   using CodeContrib.Queries;

   public class CountTeamsProjection : IProjection
   {
      public CountTeamsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}