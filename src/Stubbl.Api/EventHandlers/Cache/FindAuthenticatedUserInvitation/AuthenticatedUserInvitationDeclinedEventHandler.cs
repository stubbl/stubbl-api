using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.AuthenticatedUserInvitationDeclined.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUserInvitation
{
    public class
        AuthenticatedUserInvitationDeclinedEventHandler : IEventHandler<AuthenticatedUserInvitationDeclinedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public AuthenticatedUserInvitationDeclinedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(AuthenticatedUserInvitationDeclinedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindAuthenticatedUserInvitation
            (
                _authenticatedUserAccessor.AuthenticatedUser.EmailAddress,
                @event.InvitationId
            ));

            return Task.CompletedTask;
        }
    }
}