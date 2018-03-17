using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamMember
{
    public class
        AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _invitationsCollection = invitationsCollection;
        }

        public async Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event,
            CancellationToken cancellationToken)
        {
            var teamId = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
                .Project(i => i.Team.Id)
                .SingleAsync(cancellationToken);

            _cache.Remove(_cacheKey.FindTeamMember
            (
                teamId,
                _authenticatedUserAccessor.AuthenticatedUser.Id
            ));
        }
    }
}