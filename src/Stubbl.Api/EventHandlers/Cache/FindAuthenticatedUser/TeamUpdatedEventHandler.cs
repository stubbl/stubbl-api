using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.TeamUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUser
{
    public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Member> _membersCollection;

        public TeamUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _membersCollection = membersCollection;
        }

        public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var subs = await _membersCollection.Find(m => m.Teams.Any(t => t.Id == @event.TeamId))
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