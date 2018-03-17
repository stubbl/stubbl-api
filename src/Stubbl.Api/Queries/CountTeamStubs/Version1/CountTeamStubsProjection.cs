using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeamStubs.Version1
{
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