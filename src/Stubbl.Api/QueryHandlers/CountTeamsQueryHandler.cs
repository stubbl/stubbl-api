using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Queries.CountTeams.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountTeamsQueryHandler : IQueryHandler<CountTeamsQuery, CountTeamsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public CountTeamsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public Task<CountTeamsProjection> HandleAsync(CountTeamsQuery query, CancellationToken cancellationToken)
        {
            var projection = new CountTeamsProjection
            (
                _authenticatedUserAccessor.AuthenticatedUser.Teams.Count
            );

            return Task.FromResult(projection);
        }
    }
}