using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamStubDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.CountTeamStubs
{
    public class TeamStubDeletedEventHandler : IEventHandler<TeamStubDeletedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamStubDeletedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamStubDeletedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.CountTeamStubs
            (
                @event.TeamId
            ));

            return Task.CompletedTask;
        }
    }
}