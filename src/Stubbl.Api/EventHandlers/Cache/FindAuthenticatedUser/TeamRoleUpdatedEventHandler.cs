using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUser
{
    public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey,
            IMongoCollection<User> usersCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var subs = await _usersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
                .Project(m => m.Sub)
                .ToListAsync(cancellationToken);

            foreach (var sub in subs)
            {
                _cache.Remove(_cacheKey.FindAuthenticatedUser
                (
                    sub
                ));
            }
        }
    }
}