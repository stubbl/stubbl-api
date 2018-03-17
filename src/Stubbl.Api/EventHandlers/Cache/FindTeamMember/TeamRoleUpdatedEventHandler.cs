using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamMember
{
    public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Member> _membersCollection;

        public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _membersCollection = membersCollection;
        }

        public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var memberIds = await _membersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
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