using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamStubCreated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.CountTeamStubs
{
    public class TeamStubCreatedEventHandler : IEventHandler<TeamStubCreatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamStubCreatedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamStubCreatedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.CountTeamStubs
            (
                @event.TeamId
            ));

            return Task.CompletedTask;
        }
    }
}