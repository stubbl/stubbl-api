using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamMember
{
    public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<User> usersCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var memberIds = await _usersCollection.Find(m => m.Teams.Any(t => t.Id == @event.TeamId))
                .Project(m => m.Id)
                .ToListAsync(cancellationToken);

            foreach (var memberId in memberIds)
            {
                _cache.Remove(_cacheKey.FindTeamMember
                (
                    @event.TeamId,
                    memberId
                ));
            }
        }
    }
}