using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Queries.CountTeamMembers.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountTeamMembersQueryHandler : IQueryHandler<CountTeamMembersQuery, CountTeamMembersProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public CountTeamMembersQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<CountTeamMembersProjection> HandleAsync(CountTeamMembersQuery query,
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

            var totalCount = await _teamsCollection.Find(t => t.Id == query.TeamId)
                .Project(t => t.Members.Count())
                .SingleOrDefaultAsync();

            return new CountTeamMembersProjection
            (
                totalCount
            );
        }
    }
}