using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Queries.ListAuthenticatedUserTeamInvitations.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Invitation = Stubbl.Api.Data.Collections.Invitations.Invitation;

namespace Stubbl.Api.QueryHandlers
{
    public class ListAuthenticatedUserTeamInvitationsQueryHandler : IQueryHandler<ListAuthenticatedUserTeamInvitationsQuery,
        ListAuthenticatedUserTeamInvitationsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public ListAuthenticatedUserTeamInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<ListAuthenticatedUserTeamInvitationsProjection> HandleAsync(
            ListAuthenticatedUserTeamInvitationsQuery query, CancellationToken cancellationToken)
        {
            return new ListAuthenticatedUserTeamInvitationsProjection
            (
                await _invitationsCollection.Find(i =>
                        i.EmailAddress.ToLower() == _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower())
                    .Project(i => new Queries.ListAuthenticatedUserTeamInvitations.Version1.Invitation
                    (
                        i.Id.ToString(),
                        new Team
                        (
                            i.Team.Id.ToString(),
                            i.Team.Name
                        ),
                        new Role
                        (
                            i.Role.Id.ToString(),
                            i.Role.Name
                        ),
                        i.Status.ToInvitationStatus()
                    ))
                    .ToListAsync(cancellationToken)
            );
        }
    }
}