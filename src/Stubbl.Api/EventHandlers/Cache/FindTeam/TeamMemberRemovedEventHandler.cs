using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamMemberRemoved.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeam
{
    public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamMemberRemovedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindTeam
            (
                @event.TeamId
            ));

            return Task.CompletedTask;
        }
    }
}