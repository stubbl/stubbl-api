using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.AuthenticatedUserUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeam
{
    public class AuthenticatedUserUpdatedEventHandler : IEventHandler<AuthenticatedUserUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public AuthenticatedUserUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(AuthenticatedUserUpdatedEvent @event, CancellationToken cancellationToken)
        {
            foreach (var teamId in _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => t.Id))
            {
                _cache.Remove(_cacheKey.FindTeamMember
                (
                    teamId,
                    _authenticatedUserAccessor.AuthenticatedUser.Id
                ));
            }

            return Task.CompletedTask;
        }
    }
}