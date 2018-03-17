using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.QueryHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Queries.CountTeamRoles.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountTeamRolesQueryHandler : IQueryHandler<CountTeamRolesQuery, CountTeamRolesProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public CountTeamRolesQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<CountTeamRolesProjection> HandleAsync(CountTeamRolesQuery query,
            CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new MemberNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            var totalCount = await _teamsCollection.Find(t => t.Id == query.TeamId)
                .Project(t => t.Roles.Count())
                .SingleOrDefaultAsync();

            return new CountTeamRolesProjection
            (
                totalCount
            );
        }
    }
}