using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.CountAuthenticatedUserInvitations.Version1
{
    public class CountAuthenticatedUserInvitationsProjection : IProjection
    {
        public CountAuthenticatedUserInvitationsProjection(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}