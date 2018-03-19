using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamStubUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamStub
{
    public class TeamStubUpdatedEventHandler : IEventHandler<TeamStubUpdatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamStubUpdatedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamStubUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindTeamStub
            (
                @event.TeamId,
                @event.StubId
            ));

            return Task.CompletedTask;
        }
    }
}