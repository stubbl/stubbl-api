using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.QueryHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.TeamNotFound.Version1;
using Stubbl.Api.Queries.FindTeam.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class FindTeamQueryHandler : IQueryHandler<FindTeamQuery, FindTeamProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Team> _teamsCollection;

        public FindTeamQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _teamsCollection = teamsCollection;
        }

        public async Task<FindTeamProjection> HandleAsync(FindTeamQuery query, CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new MemberNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            var team = await _cache.GetOrSetAsync
            (
                _cacheKey.FindTeam(query.TeamId),
                async () => await _teamsCollection.Find(t => t.Id == query.TeamId)
                    .Project(t => new FindTeamProjection
                    (
                        t.Id.ToString(),
                        t.Name
                    ))
                    .SingleOrDefaultAsync(cancellationToken)
            );

            if (team == null)
            {
                throw new TeamNotFoundException
                (
                    query.TeamId
                );
            }

            return team;
        }
    }
}