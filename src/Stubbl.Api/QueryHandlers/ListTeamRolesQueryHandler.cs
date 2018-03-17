using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.QueryHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Queries.ListTeamRoles.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Role = Stubbl.Api.Queries.ListTeamRoles.Version1.Role;

namespace Stubbl.Api.QueryHandlers
{
    public class ListTeamRolesQueryHandler : IQueryHandler<ListTeamRolesQuery, ListTeamRolesProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public ListTeamRolesQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<ListTeamRolesProjection> HandleAsync(ListTeamRolesQuery query,
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

            var teamRoles = await _teamsCollection.Find(t => t.Id == query.TeamId)
                .Project(t => t.Roles)
                .SingleAsync(cancellationToken);

            return new ListTeamRolesProjection
            (
                teamRoles.Select(r => new Role
                    (
                        r.Id.ToString(),
                        r.Name,
                        r.Permissions.Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
                    ))
                    .ToList()
            );
        }
    }
}