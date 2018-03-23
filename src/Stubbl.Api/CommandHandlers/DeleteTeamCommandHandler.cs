using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeleteTeam.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Events.TeamDeleted.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.CommandHandlers
{
    public class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, TeamDeletedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public DeleteTeamCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamDeletedEvent> HandleAsync(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var team = _authenticatedUserAccessor.AuthenticatedUser.Teams.SingleOrDefault(t => t.Id == command.TeamId);

            if (team == null)
            {
                throw new UserNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    command.TeamId
                );
            }

            if (!team.Role.Permissions.Contains(Permission.ManageTeams))
            {
                throw new MemberCannotManageTeamsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);

            await _teamsCollection.DeleteOneAsync(filter, cancellationToken);

            return new TeamDeletedEvent
            (
                team.Id
            );
        }
    }
}