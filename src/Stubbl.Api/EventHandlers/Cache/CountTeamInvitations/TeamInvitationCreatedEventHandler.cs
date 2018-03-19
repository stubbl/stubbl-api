using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamInvitationCreated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.CountTeamInvitations
{
    public class TeamInvitationCreatedEventHandler : IEventHandler<TeamInvitationCreatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamInvitationCreatedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamInvitationCreatedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.CountTeamInvitations
            (
                @event.TeamId
            ));

            return Task.CompletedTask;
        }
    }
}