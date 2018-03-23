using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Queries.ListTeamInvitations.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Invitation = Stubbl.Api.Data.Collections.Invitations.Invitation;

namespace Stubbl.Api.QueryHandlers
{
    public class
        ListTeamInvitationsQueryHandler : IQueryHandler<ListTeamInvitationsQuery, ListTeamInvitationsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public ListTeamInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<ListTeamInvitationsProjection> HandleAsync(ListTeamInvitationsQuery query,
            CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new UserNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            return new ListTeamInvitationsProjection
            (
                await _invitationsCollection.Find(i => i.Team.Id == query.TeamId)
                    .Project(i => new Queries.ListTeamInvitations.Version1.Invitation
                    (
                        i.Id.ToString(),
                        i.Team.Id.ToString(),
                        new Role
                        (
                            i.Role.Id.ToString(),
                            i.Role.Name
                        ),
                        i.Status.ToInvitationStatus(),
                        i.EmailAddress
                    ))
                    .ToListAsync(cancellationToken)
            );
        }
    }
}