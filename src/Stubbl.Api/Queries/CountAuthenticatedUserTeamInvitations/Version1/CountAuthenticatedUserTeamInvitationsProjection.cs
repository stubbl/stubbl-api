using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountAuthenticatedUserTeamInvitations.Version1
{
    public class CountAuthenticatedUserTeamInvitationsProjection : IProjection
    {
        public CountAuthenticatedUserTeamInvitationsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}