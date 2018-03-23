using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Queries.CountTeamStubs.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountTeamStubsQueryHandler : IQueryHandler<CountTeamStubsQuery, CountTeamStubsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Stub> _stubsCollection;

        public CountTeamStubsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Stub> stubsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _stubsCollection = stubsCollection;
        }

        public async Task<CountTeamStubsProjection> HandleAsync(CountTeamStubsQuery query,
            CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new UserNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            var tags = await _cache.GetOrSetAsync
            (
                _cacheKey.CountTeamInvitations(query.TeamId),
                async () => await _stubsCollection.Find(s => s.TeamId == query.TeamId)
                    .Project(s => s.Tags)
                    .ToListAsync()
            );

            var tagCounts = tags.SelectMany(t => t)
                .GroupBy(g => g)
                .Select(g => new TagCount
                (
                    g.Key,
                    g.Count()
                ))
                .ToList();

            return new CountTeamStubsProjection
            (
                tags.Count,
                tagCounts
            );
        }
    }
}