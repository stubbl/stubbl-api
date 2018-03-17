using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountTeamInvitations.Version1
{
    public class CountTeamInvitationsProjection : IProjection
    {
        public CountTeamInvitationsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}