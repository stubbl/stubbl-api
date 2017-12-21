namespace Stubbl.Api.Core.Queries.CountTeamStubs.Version1
{
   using System.Collections.Generic;
   using Common.Queries;

   public class CountTeamStubsProjection : IProjection
   {
      public CountTeamStubsProjection(long totalCount, IReadOnlyCollection<TagCount> tagCounts)
      {
         TotalCount = totalCount;
         TagCounts = tagCounts;
      }

      public long TotalCount { get; }
      public IReadOnlyCollection<TagCount> TagCounts { get; }
   }
}