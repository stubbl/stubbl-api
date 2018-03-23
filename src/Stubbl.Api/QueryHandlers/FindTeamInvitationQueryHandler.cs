using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Exceptions.InvitationNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Queries.FindTeamInvitation.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Role = Stubbl.Api.Queries.FindTeamInvitation.Version1.Role;

namespace Stubbl.Api.QueryHandlers
{
    public class FindTeamInvitationQueryHandler : IQueryHandler<FindTeamInvitationQuery, FindTeamInvitationProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public FindTeamInvitationQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<FindTeamInvitationProjection> HandleAsync(FindTeamInvitationQuery query,
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

            var invitation = await _cache.GetOrSetAsync
            (
                _cacheKey.FindTeamInvitation(query.TeamId, query.InvitationId),
                async () => await _invitationsCollection
                    .Find(i => i.Team.Id == query.TeamId && i.Id == query.InvitationId)
                    .Project(i => new FindTeamInvitationProjection
                    (
                        i.Id.ToString(),
                        new Role
                        (
                            i.Role.Id.ToString(),
                            i.Role.Name
                        ),
                        i.Status.ToInvitationStatus()
                    ))
                    .SingleOrDefaultAsync(cancellationToken)
            );

            if (invitation == null)
            {
                throw new InvitationNotFoundException
                (
                    query.InvitationId,
                    query.TeamId
                );
            }

            return invitation;
        }
    }
}