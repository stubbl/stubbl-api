using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.ListAuthenticatedUserTeamInvitations.Version1
{
    public class ListAuthenticatedUserTeamInvitationsProjection : IProjection
    {
        public ListAuthenticatedUserTeamInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
        {
            Invitations = invitations;
        }

        public IReadOnlyCollection<Invitation> Invitations { get; }
    }
}