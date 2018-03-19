using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamRoleDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamRole
{
    public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamRoleDeletedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindTeamRole
            (
                @event.TeamId,
                @event.RoleId
            ));

            return Task.CompletedTask;
        }
    }
}