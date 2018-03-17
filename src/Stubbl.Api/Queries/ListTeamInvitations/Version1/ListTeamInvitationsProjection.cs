using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.ListTeamInvitations.Version1
{
    public class ListTeamInvitationsProjection : IProjection
    {
        public ListTeamInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
        {
            Invitations = invitations;
        }

        public IReadOnlyCollection<Invitation> Invitations { get; }
    }
}