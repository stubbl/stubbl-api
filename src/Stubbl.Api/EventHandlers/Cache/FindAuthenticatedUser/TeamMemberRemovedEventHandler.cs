using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamMemberRemoved.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUser
{
    public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamMemberRemovedEventHandler(ICache cache, ICacheKey cacheKey,
            IMongoCollection<User> usersCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
        {
            var sub = await _usersCollection.Find(m => m.Id == @event.MemberId)
                .Project(m => m.Sub)
                .SingleAsync(cancellationToken);

            _cache.Remove(_cacheKey.FindAuthenticatedUser
            (
                sub
            ));
        }
    }
}