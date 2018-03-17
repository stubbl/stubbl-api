using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.ListAuthenticatedUserInvitations.Version1
{
    public class ListAuthenticatedUserInvitationsProjection : IProjection
    {
        public ListAuthenticatedUserInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
        {
            Invitations = invitations;
        }

        public IReadOnlyCollection<Invitation> Invitations { get; }
    }
}