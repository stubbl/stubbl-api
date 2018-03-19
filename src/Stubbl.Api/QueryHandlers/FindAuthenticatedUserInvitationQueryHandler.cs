using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Exceptions.MemberNotInvitedToTeam.Version1;
using Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1;
using Role = Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1.Role;
using Team = Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1.Team;

namespace Stubbl.Api.QueryHandlers
{
    public class FindAuthenticatedUserInvitationQueryHandler : IQueryHandler<FindAuthenticatedUserInvitationQuery,
        FindAuthenticatedUserInvitationProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public FindAuthenticatedUserInvitationQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<FindAuthenticatedUserInvitationProjection> HandleAsync(
            FindAuthenticatedUserInvitationQuery query, CancellationToken cancellationToken)
        {
            var invitation = await _cache.GetOrSetAsync
            (
                _cacheKey.FindAuthenticatedUserInvitation(_authenticatedUserAccessor.AuthenticatedUser.EmailAddress,
                    query.InvitationId),
                async () => await _invitationsCollection.Find(i =>
                        i.Id == query.InvitationId &&
                        i.EmailAddress.ToLower() ==
                        _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower() &&
                        i.Status == InvitationStatus.Sent)
                    .SingleOrDefaultAsync(cancellationToken)
            );

            if (invitation == null)
            {
                throw new MemberNotInvitedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.InvitationId
                );
            }

            return new FindAuthenticatedUserInvitationProjection
            (
                invitation.Id.ToString(),
                new Team
                (
                    invitation.Team.Id.ToString(),
                    invitation.Team.Name
                ),
                new Role
                (
                    invitation.Role.Id.ToString(),
                    invitation.Role.Name
                )
            );
        }
    }
}