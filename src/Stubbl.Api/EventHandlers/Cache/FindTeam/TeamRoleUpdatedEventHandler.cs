using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeam
{
    public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Team> _teamsCollection;

        public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Team> teamsCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _teamsCollection = teamsCollection;
        }

        public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var teamIds = await _teamsCollection.Find(t => t.Roles.Any(r => r.Id == @event.RoleId))
                .Project(t => t.Id)
                .ToListAsync(cancellationToken);

            foreach (var teamId in teamIds)
            {
                _cache.Remove(_cacheKey.FindTeam
                (
                    teamId
                ));
            }
        }
    }
}